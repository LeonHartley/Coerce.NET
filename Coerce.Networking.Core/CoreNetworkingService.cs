using Coerce.Networking.Api;
using System;
using System.Collections.Generic;
using System.Text;
using Coerce.Networking.Api.Channels;
using Coerce.Networking.Api.Context;

namespace Coerce.Networking.Core
{
    public class CoreNetworkingService : INetworkingService
    {
        private IChannelInitialiser _channelInitialiser;

        private String _hostName;
        private int _port;

        public void Configure(IChannelInitialiser channelInitialiser)
        {
            this._channelInitialiser = channelInitialiser;
        }

        public void Start(string hostName, int port, Action<ServiceInitialisedContext> onServiceInitialised)
        {
            this._hostName = hostName;
            this._port = port;

            onServiceInitialised.Invoke(new ServiceInitialisedContext(this));
        }
    }
}
