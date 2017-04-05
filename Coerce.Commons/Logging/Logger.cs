using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Commons.Logging
{
    public class Logger
    {
        public string Name
        {
            get;
            private set;
        }

        public Logger(string name)
        {
            this.Name = name;
        }

        public void Debug(string message, params object[] args)
        {
            this.Log(LogLevel.DEBUG, message, args);
        }

        public void Error(string message, params object[] args)
        {
            this.Log(LogLevel.ERROR, message, args);
        }

        public void Info(string message, params object[] args)
        {
            this.Log(LogLevel.INFO, message, args);
        }

        public void Log(LogLevel level, string message, params object[] args)
        {
            foreach(LoggerAppender loggerAppender in LoggerService.Instance.Config.Appenders)
            {
                loggerAppender.Log(this, level, message, args);
            }
        }

        public void Trace(string message, params object[] args)
        {
            this.Log(LogLevel.TRACE, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            this.Log(LogLevel.WARN, message, args);
        }
    }
}
