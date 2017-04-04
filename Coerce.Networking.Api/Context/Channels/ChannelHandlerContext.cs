using Coerce.Networking.Api.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Networking.Api.Context.Channels
{
    public class ChannelHandlerContext : INetworkingContext
    {
        private Channel _channel;

        public Channel Channel
        {
            get
            {
                return this._channel;
            }
        }

        public ChannelHandlerContext(Channel channel)
        {
            this._channel = channel;
        }
    }
}
