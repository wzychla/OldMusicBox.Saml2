using OldMusicBox.Saml2.Constants;
using OldMusicBox.Saml2.Model;
using OldMusicBox.Saml2.Model.Logout;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OldMusicBox.Saml2.DemoClient.Controllers
{
    /// <summary>
    /// Home controller.
    /// Views refer to the currently logged user, stored
    /// in this.User.Identity.Name
    /// </summary>
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Logout that is triggered by this application.
        /// Please refer to the explanation provided in the comments
        /// to the Account/Logout method
        /// </summary>
        [Authorize]
        public ActionResult Logout()
        {
            var assertionIssuer             = ConfigurationManager.AppSettings["AssertionIssuer"];
            var identityProvider            = ConfigurationManager.AppSettings["IdentityProvider"];

            var requestBinding              = Binding.POST;

            var x509Configuration = new X509Configuration()
            {
                SignatureCertificate = new ClientCertificateProvider().GetClientCertificate(),
                IncludeKeyInfo       = true,
                SignatureAlgorithm   = Signature.SignatureAlgorithm.SHA256
            };

            // LogoutnRequest factory
            var logoutRequestFactory = new LogoutRequestFactory();

            logoutRequestFactory.Issuer      = assertionIssuer;
            logoutRequestFactory.Destination = identityProvider;

            logoutRequestFactory.RequestBinding    = requestBinding;
            logoutRequestFactory.X509Configuration = x509Configuration;

            logoutRequestFactory.NameID            = new NameID()
            {
                Text = this.User.Identity.Name
            };

            // the identity provider possibly needs the SessionIndex, too
            // note that the SessionIndex is obtained in the Account/Logon
            // and stored for the current session

            switch (logoutRequestFactory.RequestBinding)
            {
                case Constants.Binding.POST:
                    return Content(logoutRequestFactory.CreatePostBindingContent());
                default:
                    throw new ArgumentException(string.Format("The {0} logout request binding is not supported", logoutRequestFactory.RequestBinding));
            }
        }
    }
}