using Coerce.Networking.Core.Channels;
using System.Net.Sockets;
using System;
using System.Text;
using Coerce.Networking.Api.Context.Channels;
using Coerce.Networking.Api.Buffer;
using Coerce.Networking.Api.Buffer.Default;

namespace Coerce.Networking.Core.Sockets
{
    partial class AsyncServerSocket
    {
        /// <summary>
        /// Processes an accept operation
        /// </summary>
        /// <param name="acceptEventArgs"></param>
        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            this.StartAccept();

            if(acceptEventArgs.SocketError != SocketError.Success)
            {
                this.CancelAccept(acceptEventArgs);
                return;
            }

            SocketAsyncEventArgs ioArgs = this._ioArgsPool.Take();
            
            if(ioArgs != null)
            {
                ChannelToken channelToken = ioArgs.UserToken as ChannelToken;
                DefaultChannel channel = channelToken.Channel as DefaultChannel;

                channel.Socket = acceptEventArgs.AcceptSocket;
                channel.SendArgs.AcceptSocket = acceptEventArgs.AcceptSocket;

                acceptEventArgs.AcceptSocket = null;
                this._acceptArgsPool.Return(acceptEventArgs);
            
                StartReceive(ioArgs);

                channel.Pipeline.OnChannelConnected(new ChannelHandlerContext(channel));
            }
        }

        /// <summary>
        /// Processes a receive operation
        /// </summary>
        /// <param name="receiveEventArgs">The arguments which correspond to the receive operation</param>
        private void ProcessReceive(SocketAsyncEventArgs receiveEventArgs)
        {
            ChannelToken channelToken = receiveEventArgs.UserToken as ChannelToken;

            if (receiveEventArgs.BytesTransferred > 0 && receiveEventArgs.SocketError == SocketError.Success)
            {
                byte[] dataReceived = new byte[receiveEventArgs.BytesTransferred];

                Buffer.BlockCopy(receiveEventArgs.Buffer, receiveEventArgs.Offset, dataReceived, 0, receiveEventArgs.BytesTransferred);

                _log.Trace("Received buffer {0}", Encoding.UTF8.GetString(dataReceived));

                channelToken.Channel.Pipeline.OnChannelDataReceived(
                    new DefaultBuffer(dataReceived, dataReceived.Length, 0),
                    new ChannelHandlerContext(channelToken.Channel));

                this.StartReceive(receiveEventArgs);
            }
            else
            {
                // Disconnect socket!
                _log.Trace("Client socket closed");

                channelToken.Channel.Pipeline.OnChannelDisconnected(new ChannelHandlerContext(channelToken.Channel));
                channelToken.Channel.Dispose();

                this.CancelReceive(receiveEventArgs);
            }
        }

        /// <summary>
        /// Processes a flush operation
        /// </summary>
        /// <param name="flushArgs">The arguments which correspond to the flush operation</param>
        private void ProcessFlush(SocketAsyncEventArgs flushArgs)
        {
            ChannelToken channelToken = flushArgs.UserToken as ChannelToken;

            // We're ready to send
            if (flushArgs.SocketError == SocketError.Success)
            {
                channelToken.DataWriter.DataRemaining = channelToken.DataWriter.DataRemaining - flushArgs.BytesTransferred;

                if (channelToken.DataWriter.DataRemaining == 0)
                {
                    channelToken.DataWriter.Reset();
                    (channelToken.Channel as DefaultChannel).OnFlushComplete();
                }
                else
                {
                    channelToken.DataWriter.DataProcessed += flushArgs.BytesTransferred;
                    this.StartFlush(flushArgs);
                }
            }
        }
    }
}
