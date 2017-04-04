using Coerce.Networking.Api.Channels;
using Coerce.Networking.Api.Context.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Networking.Api.Pipeline.Handlers
{
    public interface IChannelHandler
    {
        void OnChannelConnected(ChannelHandlerContext context);

        void OnChannelDisconnected(ChannelHandlerContext context);

        void OnChannelEvent(ChannelEvent triggeredEvent, ChannelHandlerContext context);

        void OnChannelException(Exception exception, ChannelHandlerContext context);
    }
}
