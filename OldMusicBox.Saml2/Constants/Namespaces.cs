using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.Saml2.Constants
{
    /// <summary>
    /// Saml2 namespaces
    /// </summary>
    public class Namespaces
    {
        /// <summary>
        /// Assertion namespace
        /// </summary>
        public const string ASSERTION = "urn:oasis:names:tc:SAML:2.0:assertion";

        /// <summary>
        /// Protocol namespace
        /// </summary>
        public const string PROTOCOL = "urn:oasis:names:tc:SAML:2.0:protocol";

        /// <summary>
        /// XmlDsig namespace
        /// </summary>
        public const string XMLDSIG = "http://www.w3.org/2000/09/xmldsig#";

        /// <summary>
        /// In order to conform to common SAML namespacing, the serializer
        /// needs to be told how do to it
        /// </summary>
        public XmlSerializerNamespaces CreateSerializerNamespaces()
        {
            var serializerNamespaces = new XmlSerializerNamespaces();

            serializerNamespaces.Add("ds",    Namespaces.XMLDSIG);
            serializerNamespaces.Add("saml",  Namespaces.ASSERTION);
            serializerNamespaces.Add("samlp", Namespaces.PROTOCOL);

            return serializerNamespaces;
        }
    }
}
