using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2.Constants
{
    /// <summary>
    /// Binding constants
    /// </summary>
    public class Binding
    {
        /// <summary>
        /// REDIRECT binding
        /// </summary>
        public const string REDIRECT = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect";
        /// <summary>
        /// POST binding
        /// </summary>
        public const string POST     = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST";
    }
}
