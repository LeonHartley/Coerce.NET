using Coerce.Networking.Api.Channels;
using Coerce.Networking.Api.Context;
using System;

namespace Coerce.Networking.Api
{
    public interface INetworkingService
    {
        /// <summary>
        /// Configures the NetworkingService
        /// </summary>
        /// <param name="channelInitialiser">Initialises the channel's properties and handler pipeline, instances are shared for every client.</param>
        void Configure(IChannelInitialiser channelInitialiser);

        /// <summary>
        /// Starts the networking service on the specified host and port.
        /// </summary>
        /// <param name="hostName">The hostname the service should be listening on.</param>
        /// <param name="port">The port the service should be listening on.</param>
        /// <param name="onServiceInitialised">The callback that's to be executed once the service is fully initialised.</param>
        void Start(string hostName, int port, Action<ServiceInitialisedContext> onServiceInitialised);
    }
}
