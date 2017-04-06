using Coerce.Networking.Api.Pipeline.Handlers;
using System.Collections.Generic;
using Coerce.Networking.Api.Channels;
using Coerce.Networking.Api.Context.Channels;
using System;
using Coerce.Networking.Api.Buffer;

namespace Coerce.Networking.Api.Pipeline
{
    public class NetworkingPipeline : IChannelHandler
    {
        private Dictionary<String, IChannelHandler> _activeHandlers { get; }

        public NetworkingPipeline()
        {
            this._activeHandlers = new Dictionary<String, IChannelHandler>();
        }
   
        public void Dispose()
        {
            this._activeHandlers.Clear();
        }

        public void AddLast(String name, IChannelHandler handler)
        {
            this._activeHandlers.Add(name, handler);
        }

        public void Remove(String name)
        {
            this._activeHandlers.Remove(name);
        }

        public void OnChannelConnected(ChannelHandlerContext context)
        {
            foreach(IChannelHandler channelHandler in this._activeHandlers.Values)
            {
                channelHandler.OnChannelConnected(context);
            }
        }

        public void OnChannelDisconnected(ChannelHandlerContext context)
        {
            foreach(IChannelHandler channelhandler in this._activeHandlers.Values)
            {
                channelhandler.OnChannelDisconnected(context);
            }
        }

        public void OnChannelEvent(ChannelEvent triggeredEvent, ChannelHandlerContext context)
        {
            foreach (IChannelHandler channelhandler in this._activeHandlers.Values)
            {
                if (!triggeredEvent.IsComplete)
                {
                    channelhandler.OnChannelEvent(triggeredEvent, context);
                }
            }
        }

        public void OnChannelException(Exception exception, ChannelHandlerContext context)
        {
            foreach (IChannelHandler channelhander in this._activeHandlers.Values)
            {
                channelhander.OnChannelException(exception, context);
            }
        }

        public void OnChannelDataReceived(IBuffer buffer, ChannelHandlerContext context)
        {
            foreach (IChannelHandler channelhander in this._activeHandlers.Values)
            {
                if(!buffer.IsReadable())
                {
                    break;
                }

                channelhander.OnChannelDataReceived(buffer, context);
            }
        }
    }
}
