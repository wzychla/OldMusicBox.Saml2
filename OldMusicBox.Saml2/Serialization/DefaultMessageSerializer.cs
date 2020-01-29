using OldMusicBox.Saml2.Constants;
using OldMusicBox.Saml2.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace OldMusicBox.Saml2.Serialization
{
    /// <summary>
    /// Default message serializer
    /// </summary>
    public class DefaultMessageSerializer
        : IMessageSerializer
    {
        public DefaultMessageSerializer() : this( Encoding.UTF8 )
        {

        }

        public DefaultMessageSerializer( Encoding encoding )
        {
            if ( encoding == null )
            {
                throw new ArgumentNullException("encoding", "Encoding cannot be null");
            }

            this.Encoding = encoding;
        }

        public Encoding Encoding { get; set; }

        public virtual T Deserialize<T>(
            string input,
            MessageDeserializationParameters parameters)
            where T : class, ISerializableMessage
        {
            // base64?
            var data =
                parameters.ShouldDebase64Encode
                ? Convert.FromBase64String(input)
                : Encoding.GetBytes(input);

            // inflate?
            if (parameters.ShouldInflate)
            {
                data = this.Inflate(data);
            }

            var rawString = Encoding.GetString(data);

            using (var reader = new StringReader(rawString))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                var rawObject = xmlSerializer.Deserialize(reader);

                return rawObject as T;
            }
        }

        /// <summary>
        /// Saml2 serialization
        /// </summary>
        public virtual string Serialize(
            ISerializableMessage entity,
            MessageSerializationParameters parameters )
        {
            byte[] serializedBytes;

            // serialize to byte[]
            using (var writer = new StringWriter())
            {
                var xmlWriterSettings                = new XmlWriterSettings();
                xmlWriterSettings.Encoding           = this.Encoding;
                xmlWriterSettings.OmitXmlDeclaration = true;

                using (var xmlWriter = XmlWriter.Create(writer, xmlWriterSettings))
                {
                    var xmlSerializer = new XmlSerializer(entity.GetType());
                    xmlSerializer.Serialize(xmlWriter, entity, Namespaces.SerializerNamespaces);
                }

                var rawEntity = writer.ToString();

                // log
                new LoggerFactory().For(typeof(DefaultMessageSerializer)).Debug(Event.RawAuthnRequest, rawEntity);

                serializedBytes = this.Encoding.GetBytes(rawEntity);
            }

            // convert the byte[] according to parameters
            if ( parameters.ShouldDeflate )
            {
                serializedBytes = this.Deflate(serializedBytes);
            }
            if ( parameters.ShouldBase64Encode )
            {
                serializedBytes = this.Encoding.GetBytes(Convert.ToBase64String(serializedBytes));
            }

            return this.Encoding.GetString(serializedBytes);
        }

        #region Implementation

        /// <summary>
        /// Deflate/inflate
        /// </summary>
        public virtual byte[] Deflate(byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                using (var deflate = new DeflateStream(ms, CompressionMode.Compress))
                {
                    deflate.Write(bytes, 0, bytes.Length);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Deflate/inflate
        /// </summary>
        public virtual byte[] Inflate(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                using (var inflate = new DeflateStream(ms, CompressionMode.Decompress))
                {
                    using (var reader = new BinaryReader(inflate, this.Encoding))
                    {
                        return reader.ReadBytes( int.MaxValue );
                    }
                }
            }
        }

        #endregion
    }
}
