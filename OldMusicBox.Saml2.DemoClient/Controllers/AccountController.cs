using OldMusicBox.Saml2.Request;
using System;
using System.Collections.Generic;
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
                // creates the consumer assertion url (the url the IdP will redirect to)
                // this should possibly be tweaked to handle the scenario
                // your app is behind a rev proxy (so that the address should fix
                // the HTTPs scheme)
                var consumerAssertionUrl = Url.Action("Logon", "Account", null);

                var authnRequestFactory = new AuthnRequestFactory();
                var authnRequest        = authnRequestFactory.Create();

                switch ( authnRequestFactory.Binding )
                {
                    case Constants.Binding.REDIRECT:
                        return Redirect(authnRequest);
                    case Constants.Binding.POST:
                        return Content(authnRequest);
                    default:
                        throw new ArgumentException(string.Format("The {0} binding is not supported", authnRequestFactory.Binding ) );
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