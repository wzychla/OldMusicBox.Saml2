using OldMusicBox.Saml2.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OldMusicBox.Saml2.Message
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
    }
}
