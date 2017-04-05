using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Commons.Logging
{
    public abstract class LoggerAppender
    {
        public abstract void Log(Logger logger, LogLevel level, string message, params object[] args);
    }
}