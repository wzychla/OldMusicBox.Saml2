using OldMusicBox.Saml2.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2.Request
{
    /// <summary>
    /// SAML2 authentication request factory
    /// </summary>
    public class AuthnRequestFactory
    {
        public AuthnRequestFactory()
        {
            this.AuthnRequest = new AuthnRequestModel();

            this.AuthnRequest.ID           = string.Format("id_{0}", Guid.NewGuid());
            this.AuthnRequest.IssueInstant = DateTime.UtcNow;
            this.AuthnRequest.Version      = ProtocolVersion._20;

            this.Encoding                   = Encoding.UTF8;
        }

        public AuthnRequestModel AuthnRequest { get; private set; }

        /// <summary>
        /// Assertion Consumer Service URL        
        /// </summary>
        /// <remarks>
        /// In Saml2 language this is the address the Identity Provider
        /// should redirect the response to
        /// </remarks>
        public string AssertionConsumerServiceURL
        {
            get
            {
                return this.AuthnRequest.AssertionConsumerServiceURL;
            }
            set
            {
                this.AuthnRequest.AssertionConsumerServiceURL = value;
            }
        }

        /// <summary>
        /// Request issuer's name recognized by the IdentityProvider
        /// </summary>
        /// <remarks>
        /// Usually it's something different than the AssertionConsumerServiceURL
        /// </remarks>
        public string AssertionIssuer
        {
            get
            {
                return this.AuthnRequest.Issuer;
            }
            set
            {
                this.AuthnRequest.Issuer = value;
            }
        }

        /// <summary>
        /// Document's encoding
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// The Identity Provider
        /// </summary>
        public string IdentityProvider
        {
            get
            {
                return this.AuthnRequest.Destination;
            }
            set
            {
                this.AuthnRequest.Destination = value;
            }
        }

        /// <summary>
        /// Request binding
        /// </summary>
        public string RequestBinding { get; set; }

        /// <summary>
        /// Relay state
        /// </summary>
        public string RelayState { get; set; }

        /// <summary>
        /// Response binding 
        /// </summary>
        public string ResponseBinding
        {
            get
            {
                return this.AuthnRequest.ProtocolBinding;
            }
            set
            {
                this.AuthnRequest.ProtocolBinding = value;
            }
        }

        /// <summary>
        /// Certificate used to create a signature
        /// </summary>
        public X509Certificate2 SignatureCertificate { get; set; }

        /// <summary>
        /// Should the request contain the full X509KeyInfo section in the signature
        /// </summary>
        public bool IncludeX509KeyInfo { get; set; }

        /// <summary>
        /// The main method to return the AuthnRequest
        /// </summary>
        /// <remarks>
        /// Depending on the binding the result can either be
        /// * a query string the client should redirect to (REDIRECT binding)
        /// * a web page containing a form that POSTs the token to the IdP (POST binding)
        /// </remarks>
        public string BuildContent()
        {
            if ( string.IsNullOrEmpty( this.RequestBinding ) )
            {
                throw new ArgumentNullException("Request Binding cannot be null");
            }
            if (string.IsNullOrEmpty( this.ResponseBinding ) )
            {
                throw new ArgumentNullException("Response Binding cannot be null");
            }

            return string.Empty;
        }
    }
}
