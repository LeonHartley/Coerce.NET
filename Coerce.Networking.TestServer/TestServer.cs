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

        public TestServer()
        {
            this._networkingService = new CoreNetworkingService();
            this._networkingService.Configure(new ChannelInitialiser());
            
            this._networkingService.Start(this._host, this._port, (ctx) =>
            {
                Console.WriteLine("TestServer listening on tcp://{0}:{1}", this._host, this._port);
                Console.ReadKey();
            });
        }
    }
}
