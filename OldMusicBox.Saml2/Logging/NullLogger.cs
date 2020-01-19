using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2.Logging
{
    /// <summary>
    /// Null Logger
    /// </summary>
    public class NullLogger : ILogger
    {
        public void Debug(string Message)
        {
        }

        public void Error(string Message, Exception ex)
        {
        }
    }
}
