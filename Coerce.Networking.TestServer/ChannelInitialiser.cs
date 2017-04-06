using Coerce.Networking.Api.Channels;
using Coerce.Networking.Api.Pipeline.Handlers;
using System;
using System.Text;
using Coerce.Networking.Api.Context.Channels;
using Coerce.Commons.Logging;
using Coerce.Networking.Api.Buffer;
using Coerce.Networking.Api.Buffer.Default;
using System.Threading;

namespace Coerce.Networking.TestServer
{
    class ChannelHandler : IChannelHandler
    {
        private static readonly Logger _log = LoggerService.Instance.Create(nameof(ChannelHandler));
        
        public void OnChannelConnected(ChannelHandlerContext context)
        {
            _log.Debug("A channel connected");
        }

        public void OnChannelDataReceived(IBuffer buffer, ChannelHandlerContext context)
        {
            _log.Debug("Thread: {0}", Thread.CurrentThread.ManagedThreadId);

            if (UnpooledBufferAllocator.Instance.Alloc(1024, out IBuffer sendBuf))
            {
                sendBuf.WriteBytes(Encoding.UTF8.GetBytes(
                    "HTTP/1.1 200 OK\r\n" +
                    "Content-Type: text/html; charset=UTF-8\r\n" +
                    "Content-Encoding: UTF-8\r\n" +
                    "Content-Length: 16\r\n" +
                    "Server: Coerce.NET\r\n" +
                    "Connection: close\r\n" +
                    "\r\n" +
                    "send buffer test"));

                context.Channel.Write(sendBuf);

                // todo: close connection when send operation is complete
            }
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
