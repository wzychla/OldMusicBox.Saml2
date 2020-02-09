﻿using OldMusicBox.Saml2.Constants;
using OldMusicBox.Saml2.Logging;
using OldMusicBox.Saml2.Resources;
using OldMusicBox.Saml2.Serialization;
using OldMusicBox.Saml2.Signature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OldMusicBox.Saml2.Model.Logout
{
    /// <summary>
    /// Logout response factory
    /// </summary>
    public class LogoutResponseFactory
    {
        public LogoutResponseFactory()
        {
            this.LogoutResponse = new LogoutResponse();

            this.MessageSerializer = new DefaultMessageSerializer();
            this.MessageSigner     = new DefaultMessageSigner(this.MessageSerializer);

            this.LogoutResponse.ID           = string.Format("id_{0}", Guid.NewGuid());
            this.LogoutResponse.IssueInstant = DateTime.UtcNow;
            this.LogoutResponse.Version      = ProtocolVersion._20;

            this.Encoding = Encoding.UTF8;
        }

        /// <summary>
        /// Create LogoutResponse when sent by the IdP
        /// </summary>
        public LogoutResponse From( HttpRequestBase request )
        {
            if (request == null) throw new ArgumentNullException();

            var rawMessage = new RawMessageFactory().FromIdpResponse(request);
            if (rawMessage == null) return null;

            // log
            new LoggerFactory().For(this).Debug(Event.LogoutResponse, rawMessage.Payload);

            var logoutResponse        = this.MessageSerializer.Deserialize<LogoutResponse>(rawMessage.Payload, new MessageDeserializationParameters() );
            logoutResponse.RawMessage = rawMessage;
            return logoutResponse;
        }

        public LogoutResponse LogoutResponse { get; private set; }

        /// <summary>
        /// Message serializer
        /// </summary>
        public IMessageSerializer MessageSerializer { get; set; }

        /// <summary>
        /// Message signer
        /// </summary>
        public IMessageSigner MessageSigner { get; set; }

        public Encoding Encoding { get; set; }

        /// <summary>
        /// Logout request issuer
        /// </summary>
        public string Issuer
        {
            get
            {
                return this.LogoutResponse.Issuer;
            }
            set
            {
                this.LogoutResponse.Issuer = value;
            }
        }

        /// <summary>
        /// The Identity Provider
        /// </summary>
        public string Destination
        {
            get
            {
                return this.LogoutResponse.Destination;
            }
            set
            {
                this.LogoutResponse.Destination = value;
            }
        }

        /// <summary>
        /// Correlation key
        /// </summary>
        public string InResponseTo
        {
            get
            {
                return this.LogoutResponse.InResponseTo;
            }
            set
            {
                this.LogoutResponse.InResponseTo = value;
            }
        }

        /// <summary>
        /// Request binding
        /// </summary>
        public string RequestBinding { get; set; }

        /// <summary>
        /// Certificate used to create a signature
        /// </summary>
        public X509Configuration X509Configuration { get; set; }

        #region Post binding

        /// <summary>
        /// Post binding should return the AuthnRequest in a web page that posts to the identity provider
        /// </summary>
        public virtual string CreatePostBindingContent()
        {
            if (string.IsNullOrEmpty(this.RequestBinding))
            {
                throw new ArgumentNullException("RequestBinding", "Request Binding cannot be null");
            }
            if (string.IsNullOrEmpty(this.Destination))
            {
                throw new ArgumentNullException("Destination", "Destination cannot be null");
            }

            string contentPage = new ResourceFactory().Create(ResourceFactory.EmbeddedResource.ResponsePostBinding);

            contentPage = contentPage.Replace("((Destination))",  this.Destination);
            contentPage = contentPage.Replace("((SAMLResponse))", this.CreatePostBindingToken());
            contentPage = contentPage.Replace("((RelayState))",   string.Empty);

            // log
            new LoggerFactory().For(this).Debug(Event.LogoutRequest, contentPage);

            return contentPage;
        }

        /// <summary>
        /// Serialize, encode, sign, whatever necessary to create
        /// a POST binding token that is POSTed to the IdP
        /// </summary>
        /// <returns></returns>
        protected virtual string CreatePostBindingToken()
        {
            // sign the request?
            if (this.X509Configuration != null &&
                this.X509Configuration.SignatureCertificate != null
                )
            {
                var signedLogoutResponse = this.MessageSigner.Sign(this.LogoutResponse, this.X509Configuration);
                return Convert.ToBase64String(signedLogoutResponse);
            }

            throw new ArgumentException("LogoutResponse must be signed. The factory needs a non-empty X509 configuration.");
        }

        #endregion
    }
}
