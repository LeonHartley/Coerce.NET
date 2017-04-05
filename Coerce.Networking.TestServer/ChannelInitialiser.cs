using Coerce.Networking.Api.Channels;
using Coerce.Networking.Api.Pipeline.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using Coerce.Networking.Api.Context.Channels;
using Coerce.Commons.Logging;

namespace Coerce.Networking.TestServer
{
    class ChannelHandler : IChannelHandler
    {
        private static readonly Logger _log = LoggerService.Instance.Create(nameof(ChannelHandler));

        public void OnChannelConnected(ChannelHandlerContext context)
        {
            _log.Debug("A channel connected");
        }

        public void OnChannelDisconnected(ChannelHandlerContext context)
        {
            _log.Debug("A channel disconnected");
        }

        public void OnChannelEvent(ChannelEvent triggeredEvent, ChannelHandlerContext context)
        {
            _log.Debug("A channel event was triggered");
        }

        public void OnChannelException(Exception exception, ChannelHandlerContext context)
        {
            _log.Error("A channel exception was caught {0}", exception);
        }
    }

    class ChannelInitialiser : IChannelInitialiser
    {
        public void InitialiseChannel(Channel channel)
        {
            channel.Pipeline.AddLast("channelHandler", new ChannelHandler());
        }
    }
}
