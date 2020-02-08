﻿using OldMusicBox.Saml2.Constants;
using OldMusicBox.Saml2.Logging;
using OldMusicBox.Saml2.Message;
using OldMusicBox.Saml2.Model;
using OldMusicBox.Saml2.Model.Artifact;
using OldMusicBox.Saml2.Serialization;
using OldMusicBox.Saml2.Signature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OldMusicBox.Saml2
{
    /// <summary>
    /// The SAML2 authentication module
    /// </summary>
    /// <remarks>
    /// The module is designed to be used from code. 
    /// No declarative configuration  is available at this moment.
    /// 
    /// The idea here is to be as close as possible to the
    /// BCL's SessionAuthenticationModule.
    /// 
    /// Formally, the goal is to use the Saml2 in the
    /// very same way the SAM is used
    /// 
    /// https://www.wiktorzychla.com/2014/11/simplest-saml11-federated-authentication.html
    /// </remarks>
    public class Saml2AuthenticationModule 
    {
        public Saml2AuthenticationModule()
        {
            this.MessageSerializer = new DefaultMessageSerializer();
            this.MessageSigner     = new DefaultMessageSigner(this.MessageSerializer);
            this.Encoding          = Encoding.UTF8;
        }

        public IMessageSerializer MessageSerializer { get; set; }

        public IMessageSigner MessageSigner { get; set; }

        public Encoding Encoding { get; set; }

        /// <summary>
        /// The SAML2 response (AuthnResponse) is passed:
        /// * in the querystring - the REDIRECT binding
        /// * in the form - the POST binding
        /// </summary>
        public virtual bool IsSignInResponse(HttpRequestBase request)
        {
            if ( request == null)
            {
                throw new ArgumentNullException();
            }

            return
                (request.HttpMethod == "GET" && request.QueryString[Elements.SAMLARTIFACT] != null) ||
                (request.HttpMethod == "GET" && request.QueryString[Elements.SAMLRESPONSE] != null) ||
                (request.HttpMethod == "POST" && request.Form[Elements.SAMLRESPONSE] != null);
        }

        /// <summary>
        /// Obtains the token from the Request (which is an IdP's response)
        /// </summary>
        public virtual Saml2SecurityToken GetPostSecurityToken( HttpRequestBase request )
        {
            var rawMessage = new RawMessageFactory().FromIdpResponse(request);
            if ( rawMessage == null || string.IsNullOrEmpty( rawMessage.Payload )
                )
            {
                throw new ArgumentException("IdP response doesn't containt the SAML2 Response");
            }

            // log
            new LoggerFactory().For(this).Debug(Event.RawAuthnRequest, rawMessage.Payload);

            return new Saml2SecurityToken(rawMessage.Payload, this.MessageSerializer);
        }

        /// <summary>
        /// Obtains the token from the additional request made with the artifact returned by the IdP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual Saml2SecurityToken GetArtifactSecurityToken(
            HttpRequestBase              request,
            ArtifactResolveConfiguration configuration
            )
        {
            var samlArt = request.QueryString[Elements.SAMLARTIFACT];
            if ( string.IsNullOrEmpty( samlArt ) )
            {
                throw new ArgumentException("IdP response doesn't contain the SAML artifact");
            }
            if ( configuration == null ||
                 string.IsNullOrEmpty(configuration.ArtifactResolveUri) ||
                 configuration.X509Configuration == null ||
                 configuration.X509Configuration.SignatureCertificate == null
                 )
            {
                throw new ArgumentNullException("configuration", "Artifact resolution requires non empty artifact, an artifact resolution endpoint and a signing certificate");
            }

            // ArtifactResolve creation
            var artifactResolve = new ArtifactResolve();

            artifactResolve.ID           = string.Format("id_{0}", Guid.NewGuid());
            artifactResolve.Artifact     = samlArt;
            artifactResolve.IssueInstant = DateTime.UtcNow;
            artifactResolve.Issuer       = configuration.Issuer;
            artifactResolve.Version      = ProtocolVersion._20;

            var signedArtifactResolveBytes = this.MessageSigner.Sign(artifactResolve, configuration.X509Configuration);
            var signedArtifactResolveBody  = this.Encoding.GetString(signedArtifactResolveBytes).AsEnveloped();

            // log
            new LoggerFactory().For(this).Debug(Event.ArtifactResolve, signedArtifactResolveBody);

            // sending ArtifactResolve to the IdP
            var artifactClient         = new WebClient();
            artifactClient.Headers[HttpRequestHeader.ContentType] = "text/xml";

            string artifactResponseSOAP = null;
            try
            {
                // POST it
                artifactResponseSOAP = artifactClient.UploadString(configuration.ArtifactResolveUri, signedArtifactResolveBody);
            }
            catch (WebException ex)
            {
                throw new ArtifactResolveException("Artifact resolution failed because of IdP's invalid response", ex);
            }

            if (!string.IsNullOrEmpty(artifactResponseSOAP))
            {
                // log
                new LoggerFactory().For(this).Debug(Event.ArtifactResponse, artifactResponseSOAP);

                // parsing the ArtifactResponse
                return ParseArtifactResponse(artifactResponseSOAP);
            }
            else
            {
                throw new ArtifactResolveException("Empty response returned from the IdP on the ArtifactResolve call");
            }
        }

        /// <summary>
        /// Unpacks the token from the ArtifactResponse response
        /// </summary>
        private Saml2SecurityToken ParseArtifactResponse( string artifactResponseSOAP )
        {
            // unpack the SOAP envelope
            string artifactResponseString = artifactResponseSOAP.FromEnveloped();
            if ( string.IsNullOrEmpty(artifactResponseString) )
            {
                throw new ArtifactResolveException("IdP ArtifactResponse has an empty body");
            }

            // deserialize
            var artifactResponse = this.MessageSerializer.Deserialize<ArtifactResponse>(artifactResponseString, new MessageDeserializationParameters());

            // validate
            if ( artifactResponse.Status != null &&
                 artifactResponse.Status.StatusCode != null &&
                 artifactResponse.Response != null
                )
            {
                if (artifactResponse.Status.StatusCode.Value == StatusCodes.SUCCESS)
                {
                    return new Saml2SecurityToken(artifactResponse.Response.OuterXml);
                }
                else
                {
                    throw new ArtifactResolveException(string.Format("Invalid ArtifactResponse status code: {0}", artifactResponse.Status.StatusCode.Value));
                }
            }
            else
            {
                throw new ArtifactResolveException("Invalid IdP ArtifactResponse response");
            }
        }
    }
}
