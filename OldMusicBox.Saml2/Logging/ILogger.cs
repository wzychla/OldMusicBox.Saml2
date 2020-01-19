using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2.Logging
{
    /// <summary>
    /// Internal logging
    /// </summary>
    public interface ILogger
    {
        void Debug(string Message);
        void Error(string Message, Exception ex);
    }
}
