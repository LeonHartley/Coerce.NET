using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Networking.Api.Channels
{
    public interface IChannelInitialiser
    {
        /// <summary>
        /// Initialises a channel's properties and handler pipeline
        /// </summary>
        /// <param name="channel">The Channel instance that requires initialising</param>
        void InitialiseChannel(Channel channel);
    }
}
