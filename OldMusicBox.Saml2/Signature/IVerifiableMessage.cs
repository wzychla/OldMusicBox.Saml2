using OldMusicBox.Saml2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.Saml2.Signature
{
    /// <summary>
    /// Interface that marks verifiable messages
    /// </summary>
    public interface IVerifiableMessage
    {
        RawMessage RawMessage { get; set; }
    }
}
