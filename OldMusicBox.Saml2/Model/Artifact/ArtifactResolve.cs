using OldMusicBox.Saml2.Constants;
using OldMusicBox.Saml2.Signature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.Saml2.Model.Artifact
{
    /// <summary>
    /// ArtifactResolve request model
    /// </summary>
    [XmlRoot("ArtifactResolve", Namespace = Namespaces.PROTOCOL)]
    public class ArtifactResolve : ISignableMessage
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlAttribute("Version")]
        public string Version { get; set; }

        [XmlAttribute("IssueInstant")]
        public DateTime IssueInstant { get; set; }

        [XmlElement("Issuer", Namespace = Namespaces.ASSERTION)]
        public string Issuer { get; set; }

        [XmlElement("Artifact", Namespace = Namespaces.PROTOCOL)]
        public string Artifact { get; set; }
    }
}
