using OldMusicBox.Saml2.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.Saml2.Model
{
    /// <summary>
    /// XmlDsig basic model
    /// </summary>
    public class Signature
    {
        [XmlElement("KeyInfo", Namespace = Namespaces.XMLDSIG)]
        public KeyInfo KeyInfo { get; set; }
    }

    public class KeyInfo
    {
        [XmlElement("X509Data", Namespace = Namespaces.XMLDSIG)]
        public X509Data X509Data { get; set; }
    }

    public class X509Certificate
    {
        [XmlText]
        public virtual string Text { get; set; }
    }

    public class X509Data
    {
        [XmlElement("X509Certificate", Namespace = Namespaces.XMLDSIG)]
        public X509Certificate Certificate { get; set; }
    }

}
