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
        private int _id;
        private NetworkingPipeline _pipeline = new NetworkingPipeline();

        public int Id
        {
            get
            {
                return this._id;
            }

            protected set
            {
                this._id = Id;
            }
        }

        public NetworkingPipeline Pipeline
        {
            get
            {
                return this._pipeline;
            }
        }

        public string IpAddress
        {
            get
            {
                return this.GetIpAddress();
            }
        }

        public abstract void Write(IBuffer buffer);

        protected abstract string GetIpAddress();

        public void FireEvent(ChannelEvent channelEvent)
        {
            this._pipeline.OnChannelEvent(channelEvent, new ChannelHandlerContext(this));
        }
    }
}
