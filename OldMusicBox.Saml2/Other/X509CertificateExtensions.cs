using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2
{
    /// <summary>
    /// Some X509 certificate conversions
    /// </summary>
    public static class X509CertificateExtensions
    {
        /// <remarks>
        /// https://stackoverflow.com/questions/29005876/signedxml-compute-signature-with-sha256
        /// </remarks>
        public static RSACryptoServiceProvider ToSha256PrivateKey(this X509Certificate2 cert)
        {
            try
            {
                if (cert == null || cert.GetRSAPrivateKey() == null)
                    throw new ArgumentNullException();

                var exportedKeyMaterial = cert.GetRSAPrivateKey().ToXmlString(true);
                var key                 = new RSACryptoServiceProvider(new CspParameters(24 /* PROV_RSA_AES */));
                key.PersistKeyInCsp     = false;
                key.FromXmlString(exportedKeyMaterial);

                return key;
            }
            catch ( Exception ex )
            {
                throw ex;
            }
        }
    }
}
