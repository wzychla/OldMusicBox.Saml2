using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2
{
    /// <summary>
    /// The SAML2 security token handler
    /// </summary>
    public class Saml2SecurityTokenHandler : SecurityTokenHandler
    {
        public override string[] GetTokenTypeIdentifiers()
        {
            // #TODO return actual token types
            return new string[0];
        }

        public override Type TokenType
        {
            get
            {
                return typeof(Saml2SecurityToken);
            }
        }

        /// <summary>
        /// Creates
        /// </summary>
        public override ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token)
        {
            return base.ValidateToken(token);
        }
    }
}
