using System;

namespace OldMusicBox.Saml2.Message
{
    /// <summary>
    /// Raw message 
    /// 
    /// </summary>
    public class RawMessage
    {
        /// <summary>
        /// Payload
        /// </summary>
        public string Payload { get; set; }

        /// <summary>
        /// Signature (if detached)
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// RelayState
        /// </summary>
        public string RelayState { get; set; }
    }
}
