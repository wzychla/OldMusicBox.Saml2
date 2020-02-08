using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2.Logging
{
    /// <summary>
    /// Low lever events/objects that should be trackable
    /// </summary>
    public enum Event
    {
        // raw authnrequest token
        RawAuthnRequest,
        // complete post binding page
        PostBindingPage,
        // whatever comes as SAMLResponse
        RawResponse,
        // a signed message
        SignedMessage,
        // artifact resolve
        ArtifactResolve,
        // artifact response
        ArtifactResponse
    }
}
