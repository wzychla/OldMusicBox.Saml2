using OldMusicBox.Saml2.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2.Signature
{
    /// <summary>
    /// Interface that marks signable messages
    /// * AuthnRequest
    /// </summary>
    public interface ISignableMessage : ISerializableMessage
    {
        string ID { get; set; }
    }
}
