using System;
using System.Collections.Generic;
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
    }
}