using OldMusicBox.Saml2.Constants;
using OldMusicBox.Saml2.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.Saml2.Response
{
    /// <summary>
    /// Saml2 Response model
    /// </summary>
    [XmlRoot("Response", Namespace = Namespaces.PROTOCOL)]
    public class ResponseModel
        : ISerializableMessage
    {
        [XmlIgnore]
        public Assertion[] Assertions
        {
            get
            {
                if ( this.UnencryptedAssertions != null )
                {
                    return this.UnencryptedAssertions;
                }
                else
                {
                    return new Assertion[0];
                }
            }
        }

        [XmlAttribute("Consent")]
        public string Consent { get; set; }

        [XmlAttribute("Destination")]
        public string Destination { get; set; }

        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlAttribute("InResponseTo")]
        public string InResponseTo { get; set; }

        [XmlAttribute("IssueInstant")]
        public DateTime IssueInstant { get; set; }

        [XmlElement("Issuer", Namespace = Namespaces.ASSERTION)]
        public string Issuer { get; set; }

        [XmlElement("Status", Namespace = Namespaces.PROTOCOL)]
        public Status Status { get; set; }

        [XmlElement("Assertion", Namespace = Namespaces.ASSERTION)]
        public Assertion[] UnencryptedAssertions { get; set; }

        [XmlAttribute("Version")]
        public string Version { get; set; }
    }
}
