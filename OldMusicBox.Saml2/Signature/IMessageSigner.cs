using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2.Signature
{
    /// <summary>
    /// Message signer
    /// </summary>
    public interface IMessageSigner
    {
        byte[] Sign(
            ISignableMessage   message,
            X509Certificate2   x509Certificate,
            bool               x509IncludeKeyInfo,
            SignatureAlgorithm x509SignatureAlgorithm
            );
    }
}
