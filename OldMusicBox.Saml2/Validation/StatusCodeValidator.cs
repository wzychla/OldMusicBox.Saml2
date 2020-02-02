using OldMusicBox.Saml2.Constants;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2.Validation
{
    public class StatusCodeValidator : ISaml2TokenValidator
    {
        public void Validate(Saml2SecurityToken token, SecurityTokenHandlerConfiguration configuration)
        {
            if (token == null || token.Response == null || configuration == null) throw new ArgumentNullException();

            if ( 
                 token.Response.Status != null &&
                 token.Response.Status.StatusCode != null
                )
            {
                if (token.Response.Status.StatusCode.Value != StatusCodes.SUCCESS)
                {
                    throw new ValidationException(string.Format("Token status code validation error. Error reason: {0}", token.Response.Status.StatusCode.Value ));
                }
            }
        }
    }
}
