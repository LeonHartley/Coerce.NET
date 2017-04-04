using Coerce.Networking.Api.Buffer;
using Coerce.Networking.Api.Context.Channels;
using Coerce.Networking.Api.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Networking.Api.Channels
{
    public abstract class Channel
    {
        private NetworkingPipeline _pipeline;

        public NetworkingPipeline Pipeline
        {
            get
            {
                return this._pipeline;
            }
        }

        abstract public void Write(IBuffer buffer);

        public void FireEvent(ChannelEvent channelEvent)
        {
            this._pipeline.OnChannelEvent(channelEvent, new ChannelHandlerContext(this));
        }
    }
}
