using Coerce.Commons.Logging;
using Coerce.Networking.Api.Buffer;
using Coerce.Networking.Core.Buffer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Coerce.Networking.Core.Sockets
{
    class AsyncServerSocket
    { 
        // todo: allow the default settings to be overriden
        private static readonly int ChannelBufferSize = 1024;

        private static readonly int ConnectionBacklog = 64;
        private static readonly int MaxSimultaneousAcceptOperations = 20;
        private static readonly int MaxSimultaneousConnections = 128;

        private static readonly Logger _log = LoggerService.Instance.Create(nameof(AsyncServerSocket));

        private IBufferAllocator _bufferAllocator;

        private SocketAsyncEventArgsPool _acceptArgsPool;
        private SocketAsyncEventArgsPool _ioArgsPool;

        private IPEndPoint _listenEndpoint;
        private Socket _listenSocket;

        public AsyncServerSocket(IPEndPoint listenEndpoint)
        {
            this._listenEndpoint = listenEndpoint;

            this._bufferAllocator = new BufferAllocator();

            this._acceptArgsPool = new SocketAsyncEventArgsPool(MaxSimultaneousAcceptOperations, CreateAcceptEventArgs);
            //this._ioArgsPool = new SocketAsyncEventArgsPool(MaxSimultaneousConnections, CreateIoEventArgs); 
        }

        public void Listen()
        {
            this._listenSocket = new Socket(this._listenEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this._listenSocket.Bind(this._listenEndpoint);
            this._listenSocket.Listen(ConnectionBacklog);

            this.StartAccept();
        }

        public void StartAccept()
        {
            SocketAsyncEventArgs acceptEventArgs = this._acceptArgsPool.Take();

            if(acceptEventArgs != null)
            {
                bool eventPending = this._listenSocket.AcceptAsync(acceptEventArgs);

                if(!eventPending)
                {
                    this.ProcessAccept(acceptEventArgs);
                }
            }
        }

        private SocketAsyncEventArgs CreateAcceptEventArgs()
        {
            SocketAsyncEventArgs acceptArgs = new SocketAsyncEventArgs();
            acceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(ProcessOperation);

            return acceptArgs;
        }

        private SocketAsyncEventArgs CreateIoEventArgs()
        {
            return null;
        }

        private void ProcessOperation(object sender, SocketAsyncEventArgs e)
        {
            _log.Trace("Processing operation {0}", e.LastOperation);
        }

        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            this.StartAccept();

            _log.Trace("Channel accepted");
        }
    }
}
