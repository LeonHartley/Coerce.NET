using Coerce.Commons.Logging;
using Coerce.Commons.Logging.Appenders;
using Coerce.Networking.Api;
using Coerce.Networking.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Networking.TestServer
{
    class TestServer
    {
        private INetworkingService _networkingService;
        
        private readonly string _host = "0.0.0.0";
        private readonly int _port = 8080;

        private Logger _log = null;

        public TestServer()
        {
            // Create a logger.
            LoggerService loggerService = new LoggerService();

            List<LoggerAppender> appenders = new List<LoggerAppender>();
            appenders.Add(new ConsoleAppender());

            loggerService.Config.Appenders = appenders;
            LoggerService.Instance = loggerService;

            this._log = loggerService.Create(nameof(TestServer));

            this._networkingService = new CoreNetworkingService();
            this._networkingService.Configure(new ChannelInitialiser());
            
            this._networkingService.Start(this._host, this._port, (ctx) =>
            {
                this._log.Info("Server listening on tcp://{0}:{1}", this._host, this._port);
                Console.ReadKey();
            });
        }
    }
}
