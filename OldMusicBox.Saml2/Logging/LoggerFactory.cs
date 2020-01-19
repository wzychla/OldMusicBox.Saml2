﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2.Logging
{
    /// <summary>
    /// Logging Local Factory with injectable provider
    /// </summary>
    public class LoggerFactory
    {
        private static Func<ILogger> _loggerProvider;

        static LoggerFactory()
        {
            _loggerProvider = () => new NullLogger();
        }

        public static void SetProvider( Func<ILogger> loggerProvider )
        {
            if ( loggerProvider != null )
            {
                _loggerProvider = loggerProvider;
            }
            else
            {
                _loggerProvider = () => new NullLogger();
            }
        }

        public ILogger Create()
        {
            if (_loggerProvider == null) throw new ArgumentNullException();

            return _loggerProvider ();
        }
    }
}
