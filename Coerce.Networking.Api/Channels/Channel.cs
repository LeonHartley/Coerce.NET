using Coerce.Networking.Api.Buffer;
using Coerce.Networking.Api.Channels.Attachments;
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
        private ChannelAttachmentStore _attachments = new ChannelAttachmentStore();

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

        public ChannelAttachmentStore Attachments
        {
            get
            {
                return this._attachments;
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

        public abstract void Close();

        public void Dispose()
        {
            this.OnDispose();

            this.Attachments.Dispose();
        }

        public virtual void OnDispose()
        {
            // do we need to do anything else in our own channel implementations?
        }
    }
}
