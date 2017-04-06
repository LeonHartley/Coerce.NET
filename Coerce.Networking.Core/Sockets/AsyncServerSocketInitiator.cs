using Coerce.Networking.Core.Channels;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Coerce.Networking.Core.Sockets
{
    partial class AsyncServerSocket
    {
        /// <summary>
        /// Starts an accept operation
        /// </summary>
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

        /// <summary>
        /// Starts a receive operation with the provided event args
        /// </summary>
        /// <param name="receiveArgs">The data which specifies who's receiving what data</param>
        public void StartReceive(SocketAsyncEventArgs receiveArgs)
        {
            CoreChannel channel = (receiveArgs.UserToken as ChannelToken).Channel as CoreChannel;

            bool eventPending = channel.Socket.ReceiveAsync(receiveArgs);

            if(!eventPending)
            {
                this.ProcessReceive(receiveArgs);
            }
        }

        /// <summary>
        /// Start flushing the data to a channel
        /// </summary>
        /// <param name="flushArgs">The event in which to write the data to</param>
        public void StartFlush(SocketAsyncEventArgs flushArgs)
        {
            ChannelToken channelToken = flushArgs.UserToken as ChannelToken;
            int dataToFlush = channelToken.DataWriter.DataRemaining <= ChannelBufferSize ? channelToken.DataWriter.DataRemaining : ChannelBufferSize;

            flushArgs.SetBuffer(flushArgs.Offset, dataToFlush);
            
            Buffer.BlockCopy(
                channelToken.DataWriter.Data,
                channelToken.DataWriter.DataProcessed,
                flushArgs.Buffer,
                flushArgs.Offset,
                dataToFlush);

            bool eventPending = flushArgs.AcceptSocket.SendAsync(flushArgs);

            if(!eventPending)
            {
                this.ProcessFlush(flushArgs);
            }
        }
    }
}
