using OldMusicBox.Saml2.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.Saml2.Response
{
    /// <summary>
    /// Saml2 Assertion basic model
    /// </summary>
    public class Assertion
    {
        [XmlElement("AuthnStatement", Namespace = Namespaces.ASSERTION )]
        public AuthnStatement AuthnStatement { get; set; }

        [XmlElement("AttributeStatement", Namespace = Namespaces.ASSERTION)]
        public AttributeStatement AttributeStatement { get; set; }

        [XmlElement("Conditions", Namespace = Namespaces.ASSERTION)]
        public Conditions Conditions { get; set; }

        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlAttribute("IssueInstant")]
        public DateTime IssueInstant { get; set; }

        [XmlElement("Issuer", Namespace = Namespaces.ASSERTION)]
        public string Issuer { get; set; }

        [XmlElement("Signature", Namespace = Namespaces.XMLDSIG)]
        public Signature Signature { get; set; }

        [XmlElement("Subject", Namespace = Namespaces.ASSERTION)]
        public Subject Subject { get; set; }

        [XmlAttribute("Version")]
        public string Version { get; set; }
    }

    public class Attribute
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlElement("AttributeValue", Namespace = Namespaces.ASSERTION)]
        public string[] AttributeValue { get; set; }
    }

    public class AttributeStatement
    {
        [XmlElement("Attribute", Namespace = Namespaces.ASSERTION)]
        public Attribute[] Attributes { get; set; }
    }

    public class AudienceRestriction
    {
        [XmlElement("Audience", Namespace = Namespaces.ASSERTION)]
        public string[] Audience { get; set; }
    }

    public class AuthnContext
    {
        [XmlElement("AuthnContextClassRef", Namespace = Namespaces.ASSERTION)]
        public string AuthnConextClassRef { get; set; }
    }

    public class AuthnStatement
    {
        [XmlElement("AuthnContent", Namespace = Namespaces.ASSERTION)]
        public AuthnContext AuthnContext { get; set; }

        [XmlAttribute("AuthnInstant")]
        public DateTime AuthnInstant { get; set; }

        [XmlAttribute("SessionIndex")]
        public string SessionIndex { get; set; }
    }

    public class Conditions
    {
        [XmlAttribute("NotBefore")]
        public DateTime NotBefore { get; set; }

        [XmlAttribute("NotOnOrAfter")]
        public DateTime NotOnOrAfter { get; set; }

        [XmlElement("AudienceRestriction", Namespace = Namespaces.ASSERTION)]
        public virtual AudienceRestriction[] AudienceRestriction { get; set; }
    }

    public class NameID
    {
        [XmlAttribute("Format")]
        public string Format { get; set; }

        [XmlAttribute("SPNameQualifier")]
        public string SPNameQualifier { get; set; }

        [XmlAttribute("NameQualifier")]
        public string NameQualifier { get; set; }

        [XmlAttribute("SPProvidedID")]
        public string SPProvidedID { get; set; }

        [XmlText]
        public virtual string Text { get; set; }
    }

    public class Subject
    {
        [XmlElement("NameID", Namespace = Namespaces.ASSERTION)]
        public NameID NameID { get; set; }

        [XmlElement("SubjectConfirmation", Namespace = Namespaces.ASSERTION)]
        public SubjectConfirmation SubjectConfirmation { get; set; }
    }

    public class SubjectConfirmation
    {
        [XmlAttribute("Method")]
        public string Method { get; set; }

        [XmlElement("SubjectConfirmationData", Namespace = Namespaces.ASSERTION )]
        public SubjectConfirmationData SubjectConfirmationData { get; set; }
    }

    public class SubjectConfirmationData
    {
        [XmlAttribute("InResponseTo")]
        public string InResponseTo { get; set; }

        [XmlAttribute("NotBefore")]
        public DateTime NotBefore { get; set; }

        [XmlAttribute("NotOnOrAfter")]
        public DateTime NotOnOrAfter { get; set; }

        [XmlAttribute("Recipient")]
        public string Recipient { get; set; }
    }
}
