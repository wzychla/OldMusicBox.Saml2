using OldMusicBox.Saml2.Constants;
using OldMusicBox.Saml2.Request;
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
        /// <remarks>
        /// The goal is to mimic the way SAM is used
        /// </remarks>
        public ActionResult Logon()
        {
            var saml2    = new Saml2AuthenticationModule();

            // parameters
            var assertionConsumerServiceURL = ConfigurationManager.AppSettings["AssertionConsumerServiceURL"];
            var assertionIssuer             = ConfigurationManager.AppSettings["AssertionIssuer"];
            var identityProvider            = ConfigurationManager.AppSettings["IdentityProvider"];
            var artifactResolution          = ConfigurationManager.AppSettings["ArtifactResolution"];

            var requestBinding  = Binding.POST;
            var responseBinding = Binding.ARTIFACT;

            // check if this is 
            if (!saml2.IsSignInResponse(this.Request))
            {
                // AuthnRequest factory
                var authnRequestFactory = new AuthnRequestFactory();

                authnRequestFactory.AssertionConsumerServiceURL = assertionConsumerServiceURL;
                authnRequestFactory.AssertionIssuer = assertionIssuer;
                authnRequestFactory.Destination = identityProvider;

                authnRequestFactory.RequestBinding = requestBinding;
                authnRequestFactory.ResponseBinding = responseBinding;

                var authnRequestContent = authnRequestFactory.CreateBindingContent();
                switch (authnRequestFactory.RequestBinding)
                {
                    case Constants.Binding.REDIRECT:
                        return Redirect(authnRequestContent);
                    case Constants.Binding.POST:
                        return Content(authnRequestContent);
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
                        securityToken = saml2.GetArtifactSecurityToken(this.Request);
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

                // the token is validated succesfully
                var principal = new ClaimsPrincipal(identity);
                if (principal.Identity.IsAuthenticated)
                {
                    FormsAuthentication.RedirectFromLoginPage(principal.Identity.Name, false);
                }
                else
                {
                    throw new ArgumentNullException("principal", "Unauthenticated principal returned from token validation");
                }

                return new EmptyResult();
            }
        }

        public ActionResult Logout()
        {
            return new EmptyResult();
        }

        public ActionResult LocalLogout()
        {
            FormsAuthentication.SignOut();

            return new EmptyResult();
        }
    }
}