using OldMusicBox.Saml2.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OldMusicBox.Saml2.Model
{
    /// <summary>
    /// Factory
    /// </summary>
    public class RawMessageFactory
    {
        public RawMessageFactory()
        {
            this.encoding = Encoding.UTF8;
        }

        private Encoding encoding;

        /// <summary>
        /// Used when the IdP sends the SAMLResponse
        /// </summary>
        public virtual RawMessage FromIdpResponse(HttpRequestBase request)
        {
            if (request == null) throw new ArgumentNullException("request");

            // response is always base64 encoded
            var response = request.Form[Elements.SAMLRESPONSE];

            var data  = Convert.FromBase64String(response);
            var token = this.encoding.GetString(data);

            return new RawMessage()
            {
                Payload = token
            };
        }

        /// <summary>
        /// Used when the IdP sends the SAMLRequest
        /// </summary>
        public virtual RawMessage FromIdpRequest(HttpRequestBase request)
        {
            if (request == null) throw new ArgumentNullException("request");

            // response is always base64 encoded
            var response = request.Form[Elements.SAMLREQUEST];

            var data  = Convert.FromBase64String(response);
            var token = this.encoding.GetString(data);

            return new RawMessage()
            {
                Payload = token
            };
        }
    }
}
