using OldMusicBox.Saml2.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OldMusicBox.Saml2.Signature
{
    /// <summary>
    /// Default message signer
    /// </summary>
    public class DefaultMessageSigner : IMessageSigner
    {
        public DefaultMessageSigner(IMessageSerializer serializer)
        {
            if ( serializer == null )
            {
                throw new ArgumentNullException("serializer");
            }

            this.messageSerializer = serializer;
            this.encoding          = Encoding.UTF8;
        }

        private IMessageSerializer messageSerializer { get; set; }
        private Encoding encoding { get; set; }

        public byte[] Sign(ISignableMessage message, X509Certificate2 x509Certificate, bool x509IncludeKeyInfo, SignatureAlgorithm x509SignatureAlgorithm)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            if ( x509Certificate == null )
            {
                throw new ArgumentNullException("certificate");
            }

            var messageBody = this.messageSerializer.Serialize(message, new MessageSerializationParameters()
            {
                ShouldBase64Encode = false,
                ShouldDeflate      = false
            });

            var xml = new XmlDocument();
            xml.LoadXml(messageBody);

            // convert
            return this.encoding.GetBytes(xml.OuterXml);
        }
    }
}
