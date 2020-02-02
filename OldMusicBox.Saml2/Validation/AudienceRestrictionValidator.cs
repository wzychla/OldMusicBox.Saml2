using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2.Validation
{
    /// <summary>
    /// Audience restriction validator
    /// </summary>
    public class AudienceRestrictionValidator : ISaml2TokenValidator
    {
        public void Validate(Saml2SecurityToken token, SecurityTokenHandlerConfiguration configuration)
        {
#warning TODO!
        }
    }
}
