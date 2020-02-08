using OldMusicBox.Saml2.Constants;
using OldMusicBox.Saml2.Core;
using OldMusicBox.Saml2.Model;
using OldMusicBox.Saml2.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OldMusicBox.Saml2
{
    /// <summary>
    /// The SAML2 security token
    /// </summary>
    public class Saml2SecurityToken : SecurityToken
    {
        public Saml2SecurityToken()
        {
        }

        public Saml2SecurityToken(string response) :
            this( response, new DefaultMessageSerializer())
        {

        }

        /// <summary>
        /// The constructor that creates the token from the
        /// response from the Identity Provider
        /// 
        /// The response should be valid XML
        /// </summary>
        public Saml2SecurityToken( string response, IMessageSerializer serializer )
        {
            if (string.IsNullOrEmpty(response))
                throw new ArgumentNullException("response");
            if (serializer == null)
                throw new ArgumentNullException("serializer");

            try
            {
                this.RawResponse = new XmlDocument();
                this.RawResponse.PreserveWhitespace = true;
                this.RawResponse.LoadXml(response);

                var responseNode = this.RawResponse.SelectSingleNode("samlp:Response", Namespaces.DeserializerNamespaces);
                if ( responseNode == null )
                {
                    throw new Saml2Exception("The samlp:Response node not found");
                }

                this.Response = 
                    serializer.Deserialize<Response>(
                        responseNode.OuterXml,
                        new MessageDeserializationParameters()
                        {
                            ShouldDebase64Encode = false,
                            ShouldInflate        = false
                        });               
            }
            catch ( Exception ex )
            {
                throw new Saml2Exception("Error parsing server's response", ex);
            }
        }

        /// <summary>
        /// Raw XML data
        /// </summary>
        public XmlDocument RawResponse { get; private set; }

        /// <summary>
        /// Token is always built from the IdP response
        /// (parsed from RawResponse)
        /// </summary>
        public Response Response { get; private set; }

        public override string Id
        {
            get
            {
                if (this.Response != null)
                {
                    return this.Response.ID;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public override ReadOnlyCollection<SecurityKey> SecurityKeys
        {
            get;
        }

        public override DateTime ValidFrom
        {
            get
            {
                if ( Response != null &&
                     Response.Assertions != null &&
                     Response.Assertions.Count() > 0 &&
                     Response.Assertions[0].Conditions != null
                    )
                {
                    return Response.Assertions[0].Conditions.NotBefore;
                }

                return DateTime.MinValue;
            }
        }

        public override DateTime ValidTo
        {
            get
            {
                if ( Response != null &&
                     Response.Assertions != null &&
                     Response.Assertions.Count() > 0 &&
                     Response.Assertions[0].Conditions != null
                    )
                {
                    return Response.Assertions[0].Conditions.NotOnOrAfter;
                }

                return DateTime.MaxValue;
            }
        }
    }
}
