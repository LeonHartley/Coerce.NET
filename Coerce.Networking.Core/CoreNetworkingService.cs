using Coerce.Networking.Api;
using System;
using System.Collections.Generic;
using System.Text;
using Coerce.Networking.Api.Channels;
using Coerce.Networking.Api.Context;
using Coerce.Commons.Logging;
using Coerce.Networking.Core.Sockets;
using System.Net;

namespace Coerce.Networking.Core
{
    public class CoreNetworkingService : INetworkingService
    {
        private IChannelInitialiser _channelInitialiser;

        private Logger _log = LoggerService.Instance.Create(nameof(CoreNetworkingService));

        private String _hostName;
        private int _port;

        private AsyncServerSocket _serverSocket;

        public void Configure(IChannelInitialiser channelInitialiser)
        {
            this._channelInitialiser = channelInitialiser;
        }

        public void Start(string hostName, int port, Action<ServiceInitialisedContext> onServiceInitialised)
        {
            this._hostName = hostName;
            this._port = port;

            if (!IPAddress.TryParse(this._hostName, out IPAddress ipAddress))
            {
                throw new InvalidOperationException("Invalid host name detected, a valid IP address is required");
            }

            this._serverSocket = new AsyncServerSocket(new IPEndPoint(ipAddress, port), this._channelInitialiser);

            this._serverSocket.Listen();

            onServiceInitialised.Invoke(new ServiceInitialisedContext(this));
        }
    }
}
