using Coerce.Networking.Api.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Networking.Core.Sockets
{
    class ChannelToken
    {
        private readonly Channel _channel;
        private readonly DataWriter _dataWriter;

        public ChannelToken(Channel channel, DataWriter dataWriter)
        {
            this._channel = channel;
            this._dataWriter = dataWriter;
        }

        public Channel Channel
        {
            get
            {
                return this._channel;
            }
        }

        public DataWriter DataWriter
        {
            get
            {
                return this._dataWriter;
            }
        }
    }
}
