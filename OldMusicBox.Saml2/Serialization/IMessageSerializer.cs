using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2.Serialization
{
    /// <summary>
    /// Generic entity serializer/deserializer that follows Saml2 specs
    /// </summary>
    public interface IMessageSerializer
    {
        string Serialize(ISerializableMessage entity, MessageSerializationParameters parameters );
        T Deserialize<T>(string input) where T : ISerializableMessage;
    }

    public class MessageSerializationParameters
    {
        public MessageSerializationParameters() { }

        public MessageSerializationParameters(
            bool shouldBase64Encode,
            bool shouldDeflate,
            bool shouldUrlEncode
            )
        {
            this.ShouldBase64Encode = shouldBase64Encode;
            this.ShouldDeflate      = shouldDeflate;
            this.ShouldUrlEncode    = ShouldUrlEncode;
        }

        public bool ShouldBase64Encode { get; set; }
        public bool ShouldDeflate { get; set; }
        public bool ShouldUrlEncode { get; set; }
    }
}
