using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;

namespace OldMusicBox.Saml2.DemoClient
{
    /// <summary>
    /// Issuer name registry
    /// 
    /// This class accepts or rejects certificates used to sign tokens
    /// </summary>
    public class DemoClientIssuerNameRegistry : IssuerNameRegistry
    {
        public override string GetIssuerName(SecurityToken securityToken)
        {
            X509SecurityToken x509Token = securityToken as X509SecurityToken;
            if (x509Token != null)
                return x509Token.Certificate.Subject;
            else
                return null;
        }
    }
}