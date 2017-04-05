using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Commons.Logging.Appenders
{
    public class ConsoleAppender : StringAppender
    {
        public override void Log(Logger logger, LogLevel level, string message)
        {
            // TODO: allow custom formatting etc.
            Console.WriteLine(message);
        }
    }
}
