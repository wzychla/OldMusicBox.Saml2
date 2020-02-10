using OldMusicBox.Saml2.Constants;
using OldMusicBox.Saml2.Logging;
using OldMusicBox.Saml2.Model;
using OldMusicBox.Saml2.Serialization;
using OldMusicBox.Saml2.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
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

        /// <summary>
        /// Message signing
        /// </summary>
        public virtual byte[] Sign(
            ISignableMessage message, 
            X509Configuration x509Configuration)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            if ( x509Configuration == null ||
                 x509Configuration.SignatureCertificate == null 
                )
            {
                throw new ArgumentNullException("certificate");
            }

            // first, serialize to XML
            var messageBody = this.messageSerializer.Serialize(message, new MessageSerializationParameters()
            {
                ShouldBase64Encode = false,
                ShouldDeflate      = false
            });

            var xml = new XmlDocument();
            xml.LoadXml(messageBody);

            // sign the node with the id
            var reference = new Reference("#"+message.ID);
            var envelope  = new XmlDsigEnvelopedSignatureTransform(true);
            reference.AddTransform(envelope);

            // canonicalization
            var c14       = new XmlDsigExcC14NTransform();
            c14.Algorithm = SignedXml.XmlDsigExcC14NTransformUrl;
            reference.AddTransform(c14);

            // some more spells depending on SHA1 vs SHA256
            var signed                               = new SignedXml(xml);
            signed.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            switch ( x509Configuration.SignatureAlgorithm )
            {
                case SignatureAlgorithm.SHA1:
                    signed.SigningKey                 = x509Configuration.SignatureCertificate.PrivateKey;
                    signed.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;
                    reference.DigestMethod            = SignedXml.XmlDsigSHA1Url;
                    break;
                case SignatureAlgorithm.SHA256:
                    signed.SigningKey                 = x509Configuration.SignatureCertificate.ToSha256PrivateKey();
                    signed.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;
                    reference.DigestMethod            = SignedXml.XmlDsigSHA256Url;
                    break;
            }

            if ( x509Configuration.IncludeKeyInfo )
            {
                var key     = new System.Security.Cryptography.Xml.KeyInfo();
                var keyData = new KeyInfoX509Data(x509Configuration.SignatureCertificate);
                key.AddClause(keyData);
                signed.KeyInfo = key;
            }

            // show the reference
            signed.AddReference(reference);
            // create the signature
            signed.ComputeSignature();
            var signature = signed.GetXml();

            // insert the signature into the document
            var element = xml.DocumentElement.ChildNodes[0];
            xml.DocumentElement.InsertAfter(xml.ImportNode(signature, true), element);

            // log
            new LoggerFactory().For(this).Debug(Event.SignedMessage, xml.OuterXml);

            // convert
            return this.encoding.GetBytes(xml.OuterXml);
        }

        public virtual bool Validate(
            IVerifiableMessage message,
            X509Certificate2   certificate
            )
        {
            if ( message == null || message.RawMessage == null ||
                 string.IsNullOrEmpty( message.RawMessage.Payload )
                )
            {
                throw new ArgumentNullException("message");
            }
            if ( certificate == null )
            {
                throw new ArgumentNullException("certificate");
            }

            // search for signatures (possibly multiple)
            var xml = new XmlDocument();
            xml.LoadXml(message.RawMessage.Payload);

            var signatureNodes = xml.GetElementsByTagName("Signature", Namespaces.XMLDSIG);
            foreach (XmlElement signatureNode in signatureNodes)
            {
                var signedXml = new SignedXml(signatureNode.ParentNode as XmlElement);
                signedXml.LoadXml(signatureNode);
                signedXml.SafeCanonicalizationMethods.Add("http://www.w3.org/TR/1999/REC-xpath-19991116");

                var result = signedXml.CheckSignature(certificate.PublicKey.Key);

                if (!result)
                {
                    throw new ValidationException(string.Format("{0} signature validation failed", message.GetType().Name ));
                }

                return result;
            }
            return false;
        }
    }
}
