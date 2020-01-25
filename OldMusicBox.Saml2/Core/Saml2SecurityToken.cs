using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2
{
    /// <summary>
    /// The SAML2 security token
    /// </summary>
    public class Saml2SecurityToken : SecurityToken
    {
        public override string Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<SecurityKey> SecurityKeys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override DateTime ValidFrom
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override DateTime ValidTo
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
