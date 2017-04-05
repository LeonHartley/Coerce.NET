using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Commons.Logging
{
    public class LoggerService
    {
        private LoggerConfig _config = new LoggerConfig();
        
        public Logger Create(string name)
        {
            return new Logger(name);
        }

        public LoggerConfig Config
        {
            get
            {
                return this._config;
            }
        }

        public static LoggerService Instance
        {
            get;
            set;
        }
    }
}
