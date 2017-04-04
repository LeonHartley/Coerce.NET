using Coerce.Networking.Api.Channels;
using Coerce.Networking.Api.Pipeline.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using Coerce.Networking.Api.Context.Channels;

namespace Coerce.Networking.TestServer
{
    class ChannelHandler : IChannelHandler
    {
        public void OnChannelConnected(ChannelHandlerContext context)
        {
            Console.WriteLine("A channel connected");
        }

        public void OnChannelDisconnected(ChannelHandlerContext context)
        {
            Console.WriteLine("A channel disconnected");
        }

        public void OnChannelEvent(ChannelEvent triggeredEvent, ChannelHandlerContext context)
        { 
            Console.WriteLine("A channel event was triggered");
        }

        public void OnChannelException(Exception exception, ChannelHandlerContext context)
        {
            Console.WriteLine("A channel exception was caught: {0}", exception);  
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
