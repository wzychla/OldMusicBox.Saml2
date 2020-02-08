using OldMusicBox.Saml2.Signature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2.Model
{
    /// <summary>
    /// Couple of X509 parameters that usually go togheter
    /// </summary>
    public class X509Configuration
    {
        /// <summary>
        /// Signature certificate
        /// </summary>
        public X509Certificate2 SignatureCertificate { get; set; }

        public SignatureAlgorithm SignatureAlgorithm { get; set; }

        /// <summary>
        /// Should the request contain the full X509KeyInfo section in the signature
        /// </summary>
        public bool IncludeKeyInfo { get; set; }

    }
}
