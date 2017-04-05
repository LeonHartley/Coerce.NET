using Coerce.Networking.Api.Buffer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Networking.Core.Sockets
{
    class AsyncServerSocket
    {
        private static readonly int ChannelBufferSize = 1024;

        private IBufferAllocator _bufferAllocator;

        public AsyncServerSocket()
        {
            this._bufferAllocator = new IBufferAllocator();
        }
    }
}
