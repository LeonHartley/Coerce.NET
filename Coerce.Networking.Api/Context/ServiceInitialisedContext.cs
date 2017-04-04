using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Networking.Api.Context
{
    public class ServiceInitialisedContext : INetworkingContext
    {
        private INetworkingService _networkingService;

        /// <summary>
        /// The NetworkingService instance from the current context
        /// </summary>
        public INetworkingService NetworkingService
        {
            get
            {
                return this._networkingService;
            }
        }

        public ServiceInitialisedContext(INetworkingService networkingService)
        {
            this._networkingService = networkingService;
        }
    }
}
