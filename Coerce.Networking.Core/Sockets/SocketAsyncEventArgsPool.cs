using Coerce.Commons.Collections;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Coerce.Networking.Core.Sockets
{
    class SocketAsyncEventArgsPool : QueuedObjectPool<SocketAsyncEventArgs>
    {
        public SocketAsyncEventArgsPool(int size, Func<SocketAsyncEventArgs> factory)
        {
            this.Initialise(size, size, factory);
        }
    }
}
