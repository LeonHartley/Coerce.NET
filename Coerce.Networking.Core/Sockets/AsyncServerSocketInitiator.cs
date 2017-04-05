using Coerce.Networking.Core.Channels;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Coerce.Networking.Core.Sockets
{
    partial class AsyncServerSocket
    {
        public void StartAccept()
        {
            SocketAsyncEventArgs acceptEventArgs = this._acceptArgsPool.Take();

            if (acceptEventArgs != null)
            {
                bool eventPending = this._listenSocket.AcceptAsync(acceptEventArgs);

                if (!eventPending)
                {
                    this.ProcessAccept(acceptEventArgs);
                }
            }
        }

        public void StartReceive(SocketAsyncEventArgs receiveArgs)
        {
            CoreChannel channel = receiveArgs.UserToken as CoreChannel;

            bool eventPending = channel.Socket.ReceiveAsync(receiveArgs);

            if(!eventPending)
            {
                this.ProcessReceive(receiveArgs);
            }
        }
    }
}
