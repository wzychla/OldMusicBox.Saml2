using OldMusicBox.Saml2.Constants;
using OldMusicBox.Saml2.Request;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            var request = this.Request;

            // check if this is 
            if ( !saml2.IsSignInResponse( request ) )
            {
                // AuthnRequest factory
                var authnRequestFactory = new AuthnRequestFactory();

                // parameters
                var assertionConsumerServiceURL = ConfigurationManager.AppSettings["AssertionConsumerServiceURL"];
                var assertionIssuer             = ConfigurationManager.AppSettings["AssertionIssuer"];
                var identityProvider            = ConfigurationManager.AppSettings["IdentityProvider"];

                authnRequestFactory.AssertionConsumerServiceURL = assertionConsumerServiceURL;
                authnRequestFactory.AssertionIssuer             = assertionIssuer;
                authnRequestFactory.Destination                 = identityProvider;

                authnRequestFactory.RequestBinding  = Binding.REDIRECT;
                authnRequestFactory.ResponseBinding = Binding.REDIRECT;

                var authnRequestContent = authnRequestFactory.CreateBindingContent();
                switch ( authnRequestFactory.RequestBinding )
                {
                    case Constants.Binding.REDIRECT:
                        return Redirect(authnRequestContent);
                    case Constants.Binding.POST:
                        return Content(authnRequestContent);
                    default:
                        throw new ArgumentException(string.Format("The {0} request binding is not supported", authnRequestFactory.RequestBinding ) );
                }
            }
            else
            {
                // the token is created from the IdP's response
                var securityToken = saml2.GetSecurityToken(request);
                var tokenHandler  = new Saml2SecurityTokenHandler();

                // the token is validated
                var identity  = tokenHandler.ValidateToken(securityToken);
                var principal = new ClaimsPrincipal(identity);

                FormsAuthentication.RedirectFromLoginPage(principal.Identity.Name, false);

                return new EmptyResult();
            }
        }

        public ActionResult Logoff()
        {
            return new EmptyResult();
        }
    }
}