using System;
using System.Collections.Generic;
using System.Linq;
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
        public const string SAMLRESPONSE = "SamlResponse";

        /// <summary>
        /// The SAML2 response (AuthnResponse) is passed:
        /// * in the querystring - the REDIRECT binding
        /// * in the form - the POST binding
        /// </summary>
        public bool IsSignInResponse(HttpRequestBase request)
        {
            if ( request == null)
            {
                throw new ArgumentNullException();
            }

            return
                (request.HttpMethod == "GET"  && request.QueryString[SAMLRESPONSE] != null) ||
                (request.HttpMethod == "POST" && request.Form[SAMLRESPONSE] != null);
        }

        /// <summary>
        /// Obtains the token from the Request (which is an IdP's response)
        /// </summary>
        public Saml2SecurityToken GetSecurityToken( HttpRequestBase request )
        {
            return null;
        }
    }
}
