using OldMusicBox.Saml2.Constants;
using OldMusicBox.Saml2.Model;
using OldMusicBox.Saml2.Model.Artifact;
using OldMusicBox.Saml2.Model.Logout;
using OldMusicBox.Saml2.Model.Request;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OldMusicBox.Saml2.DemoClient.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Logon flow 
        /// </summary>
        public ActionResult Logon()
        {
            var saml2    = new Saml2AuthenticationModule();

            // parameters
            var assertionConsumerServiceURL = ConfigurationManager.AppSettings["AssertionConsumerServiceURL"];
            var assertionIssuer             = ConfigurationManager.AppSettings["AssertionIssuer"];
            var identityProvider            = ConfigurationManager.AppSettings["IdentityProvider"];
            var artifactResolve             = ConfigurationManager.AppSettings["ArtifactResolve"];

            var requestBinding  = Binding.POST;
            var responseBinding = Binding.POST;

            // this is optional
            var x509Configuration = new X509Configuration()
            {
                SignatureCertificate = new ClientCertificateProvider().GetClientCertificate(),
                IncludeKeyInfo       = true,
                SignatureAlgorithm   = Signature.SignatureAlgorithm.SHA256
            };

            // check if this is 
            if (!saml2.IsSignInResponse(this.Request))
            {
                // AuthnRequest factory
                var authnRequestFactory = new AuthnRequestFactory();

                authnRequestFactory.AssertionConsumerServiceURL = assertionConsumerServiceURL;
                authnRequestFactory.AssertionIssuer             = assertionIssuer;
                authnRequestFactory.Destination                 = identityProvider;

                authnRequestFactory.X509Configuration = x509Configuration;

                authnRequestFactory.RequestBinding  = requestBinding;
                authnRequestFactory.ResponseBinding = responseBinding;

                switch (authnRequestFactory.RequestBinding)
                {
                    case Constants.Binding.REDIRECT:
                        return Redirect(authnRequestFactory.CreateRedirectBindingContent());
                    case Constants.Binding.POST:
                        return Content(authnRequestFactory.CreatePostBindingContent());
                    default:
                        throw new ArgumentException(string.Format("The {0} request binding is not supported", authnRequestFactory.RequestBinding));
                }
            }
            else
            {
                // the token is created from the IdP's response
                Saml2SecurityToken securityToken = null;

                switch (responseBinding)
                {
                    case Binding.ARTIFACT:

                        var artifactConfig = new ArtifactResolveConfiguration()
                        {
                            ArtifactResolveUri = artifactResolve,
                            Issuer             = assertionIssuer,
                            X509Configuration  = x509Configuration
                        };

                        securityToken = saml2.GetArtifactSecurityToken(this.Request, artifactConfig);
                        break;
                    case Binding.POST:
                        securityToken = saml2.GetPostSecurityToken(this.Request);
                        break;
                    default:
                        throw new NotSupportedException(string.Format("The {0} response binding is not yet supported", responseBinding));
                }

                // fail if there is no token
                if ( securityToken == null )
                {
                    throw new ArgumentNullException("No security token found in the response accoding to the Response Binding configuration");
                }

                // the token will be validated
                var configuration = new SecurityTokenHandlerConfiguration
                {
                    CertificateValidator = X509CertificateValidator.None,
                    IssuerNameRegistry   = new DemoClientIssuerNameRegistry(),
                    DetectReplayedTokens = false                    
                };
                configuration.AudienceRestriction.AudienceMode = AudienceUriMode.Never;

                var tokenHandler = new Saml2SecurityTokenHandler()
                {
                    Configuration = configuration                    
                };
                var identity     = tokenHandler.ValidateToken(securityToken);

                // this is the SessionIndex, store it if necessary
                string sessionIndex = securityToken.Assertion.ID;

                // the token is validated succesfully
                var principal = new ClaimsPrincipal(identity);
                if (principal.Identity.IsAuthenticated)
                {
                    var formsTicket = new FormsAuthenticationTicket(
                        1, principal.Identity.Name, DateTime.UtcNow, DateTime.UtcNow.Add(FormsAuthentication.Timeout), false, sessionIndex);

                    this.Response.AppendCookie(new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(formsTicket)));

                    var redirectUrl = FormsAuthentication.GetRedirectUrl(principal.Identity.Name, false);

                    return Redirect( redirectUrl );
                }
                else
                {
                    throw new ArgumentNullException("principal", "Unauthenticated principal returned from token validation");
                }
            }
        }

        /// <summary>
        /// SAML2 Logout. 
        /// 
        /// The SAML2 logout is complicated as there are two main scenarios.
        /// 
        /// Scenario 1.
        /// 
        /// It's this application that triggers the logout. 
        /// The application sends the LogoutRequest to the server
        /// and the server is supposed to answer with the LogoutResponse,
        /// sent to this endpoint (assuming the /account/logout is
        /// registered as the logout endpoint in the IdP).
        /// 
        /// In this app the LogoutRequest sending is handled in the HomeController
        /// (home/logout)
        /// 
        /// Scenario 2.
        /// 
        /// Another application triggers the logout. The server
        /// gets the LogoutRequest from this another app and
        /// sends LogoutRequest here. This app is supposed to
        /// answer with the LogoutResponse.
        /// 
        /// Both scenarios means that to handle logouts correctly,
        /// the app has to be able to both send and receive
        /// both LogoutRequest and LogoutResponse
        /// </summary>
        public ActionResult Logout()
        {
            var sam2 = new Saml2AuthenticationModule();
            string Message;

            if ( sam2.IsLogoutRequest( this.Request ) || 
                 sam2.IsLogoutResponse( this.Request )
                )
            {
                // first check if this is a LogoutResponse from the IdP
                var logoutResponse = new LogoutResponseFactory().From(this.Request);
                if ( logoutResponse != null )
                {
                    var result = sam2.MessageSigner.Validate(logoutResponse, null, out Message);
                }

                // then check if this is a LogoutRequest from the IdP
            }

            return new EmptyResult();
        }
    }
}