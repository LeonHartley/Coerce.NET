using Coerce.Commons.Logging;
using Coerce.Networking.Api.Buffer;
using Coerce.Networking.Api.Channels;
using Coerce.Networking.Core.Buffer;
using Coerce.Networking.Core.Channels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Coerce.Networking.Core.Sockets
{
    partial class AsyncServerSocket
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

        private int _channelIdIndex = 0;

        private IChannelInitialiser _channelInitialiser;

        public AsyncServerSocket(IPEndPoint listenEndpoint, IChannelInitialiser channelInitialiser)
        {
            this._listenEndpoint = listenEndpoint;
            this._channelInitialiser = channelInitialiser;

            this._bufferAllocator = new BufferAllocator();

            this._acceptArgsPool = new SocketAsyncEventArgsPool(MaxSimultaneousAcceptOperations, CreateAcceptEventArgs);
            this._ioArgsPool = new SocketAsyncEventArgsPool(MaxSimultaneousConnections, CreateIoEventArgs); 
        }

        public void Listen()
        {
            this._listenSocket = new Socket(this._listenEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this._listenSocket.Bind(this._listenEndpoint);
            this._listenSocket.Listen(ConnectionBacklog);

            this.StartAccept();
        }

        private SocketAsyncEventArgs CreateAcceptEventArgs()
        {
            SocketAsyncEventArgs acceptArgs = new SocketAsyncEventArgs();
            acceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(ProcessOperation);

            return acceptArgs;
        }

        private SocketAsyncEventArgs CreateIoEventArgs()
        {
            SocketAsyncEventArgs receiveArgs = CreateAcceptEventArgs();
            SocketAsyncEventArgs sendArgs = CreateAcceptEventArgs();

            receiveArgs.SetBuffer(this._bufferAllocator.Alloc(ChannelBufferSize).Get(), 0, ChannelBufferSize);
            sendArgs.SetBuffer(this._bufferAllocator.Alloc(ChannelBufferSize).Get(), 0, ChannelBufferSize);

            Channel channel = new CoreChannel(this._channelIdIndex++, this, sendArgs);
            ChannelToken channelToken = new ChannelToken(channel, new DataWriter());

            receiveArgs.UserToken = channelToken;
            sendArgs.UserToken = channelToken;

            this._channelInitialiser.InitialiseChannel(channel);

            return receiveArgs;
        }

        private void ProcessOperation(object sender, SocketAsyncEventArgs e)
        {
            _log.Trace("Processing operation {0}", e.LastOperation);

            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    ProcessAccept(e);
                    break;

                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;

                case SocketAsyncOperation.Send:
                    ProcessFlush(e);
                    break;
            }
        }

        public void CancelAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            acceptEventArgs.AcceptSocket.Shutdown(SocketShutdown.Both);
            this._acceptArgsPool.Return(acceptEventArgs);
        }

        public void CancelReceive(SocketAsyncEventArgs receiveEventArgs)
        {
            this._ioArgsPool.Return(receiveEventArgs);
        }
    }
}
