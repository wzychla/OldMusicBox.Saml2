using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2.Request
{
    /// <summary>
    /// SAML2 authentication request factory
    /// </summary>
    public class AuthnRequestFactory
    {
        /// <summary>
        /// Request binding
        /// </summary>
        public string Binding { get; set; }

        /// <summary>
        /// The main method to return the AuthnRequest
        /// </summary>
        /// <remarks>
        /// Depending on the binding the result can either be
        /// * a query string the client should redirect to (REDIRECT binding)
        /// * a web page containing a form that POSTs the token to the IdP (POST binding)
        /// </remarks>
        public string Create()
        {
            if ( string.IsNullOrEmpty( this.Binding ) )
            {
                throw new ArgumentNullException("Request Binding cannot be null");
            }

            return string.Empty;
        }
    }
}
