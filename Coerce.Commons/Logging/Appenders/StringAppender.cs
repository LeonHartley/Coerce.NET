using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Commons.Logging.Appenders
{
    public abstract class StringAppender : LoggerAppender
    {
        public override void Log(Logger logger, LogLevel level, string message, params object[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i].ToString();
                
                message = message.Replace($"{{{i}}}", arg);
            }

            this.Log(logger, level, $"[{DateTime.Now.ToString()}] [{logger.Name}] {level} - {message}");
        }

        public abstract void Log(Logger logger, LogLevel level, string message);
    }
}
