﻿using OldMusicBox.Saml2.Constants;
using OldMusicBox.Saml2.Serialization;
using OldMusicBox.Saml2.Signature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OldMusicBox.Saml2.Model
{
    /// <summary>
    /// SAML2 AuthnRequest model
    /// </summary>
    [XmlRoot("AuthnRequest", Namespace=Namespaces.PROTOCOL)]
    public class AuthnRequest : 
        ISignableMessage
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlAttribute("Version")]
        public string Version { get; set; }

        [XmlAttribute("IssueInstant")]
        public DateTime IssueInstant { get; set; }

        [XmlAttribute("AssertionConsumerServiceURL")]
        public string AssertionConsumerServiceURL { get; set; }

        [XmlAttribute("Destination")]
        public string Destination { get; set; }

        [XmlAttribute("ProtocolBinding")]
        public string ProtocolBinding { get; set; }

        [XmlElement("Issuer", Namespace = Namespaces.ASSERTION )]
        public string Issuer { get; set; }

        [XmlElement("NameIDPolicy", Namespace = Namespaces.PROTOCOL)]
        public NameIDPolicy NameIDPolicy { get; set; }

        [XmlElement("RequestedAuthnContext", Namespace = Namespaces.PROTOCOL )]
        public RequestAuthContext RequestedAuthnContext { get; set; }

    }

    public class NameIDPolicy
    {
        public NameIDPolicy()
        {
            this.Format = Constants.NameID.UNSPECIFIED;
        }

        [XmlAttribute("AllowCreate")]
        public bool AllowCreate { get; set; }

        [XmlAttribute("Format")]
        public string Format { get; set; }
    }

    public class RequestAuthContext
    {
        [XmlAttribute("Comparison")]
        public AuthnContextComparisonType Comparison { get; set; }

        [XmlElement("AuthnContextClassRef", Namespace = Namespaces.ASSERTION)]
        public virtual string AuthnContextClassRef { get; set; }
    }

    public enum AuthnContextComparisonType
    {
        [XmlEnum("exact")]
        Exact,
        [XmlEnum("mininum")]
        Minimum,
        [XmlEnum("maximum")]
        Maximum,
        [XmlEnum("better")]
        Better,
    }
}
