using Coerce.Networking.Api.Channels;
using System;
using System.Collections.Generic;
using System.Text;
using Coerce.Networking.Api.Buffer;
using System.Net.Sockets;
using Coerce.Networking.Core.Sockets;
using System.Threading;
using System.Collections.Concurrent;

namespace Coerce.Networking.Core.Channels
{
    class CoreChannel : Channel
    {
        /// <summary>
        /// Ensures there is only 1 write operation executed at a time
        /// </summary>
        private int WriterCount = 0;

        /// <summary>
        /// The queue of buffers to write to the client, if there's currently an on-going write operation,
        /// buffers will be queued until the next write operation is initiated.
        /// </summary>
        private ConcurrentQueue<IBuffer> WriterQueue { get; set; }

        /// <summary>
        /// The socket which this Channel instance is connected to
        /// </summary>
        public Socket Socket { get; set; }
        
        /// <summary>
        /// The main server socket instance
        /// </summary>
        public AsyncServerSocket ServerSocket { get; set; }

        /// <summary>
        /// The asynchronous event to be used for sending data to the channel
        /// </summary>
        public SocketAsyncEventArgs SendArgs { get; set; }

        /// <summary>
        /// Creates a channel instance
        /// </summary>
        /// <param name="channelId">The ID of the channel</param>
        /// <param name="sendEventArgs">The args used to send args</param>
        public CoreChannel(int channelId, AsyncServerSocket serverSocket, SocketAsyncEventArgs sendEventArgs)
        {
            this.Id = channelId;
            this.ServerSocket = serverSocket;
            this.SendArgs = sendEventArgs;

            this.WriterQueue = new ConcurrentQueue<IBuffer>();
        }

        /// <summary>
        /// Writes to the channel
        /// </summary>
        /// <param name="buffer">The buffer in which to write to the channel</param>
        public override void Write(IBuffer buffer)
        {
            this.WriterQueue.Enqueue(buffer);

            // start sending the data
            if(Interlocked.CompareExchange(ref WriterCount, 1, 0) == 0)
            {
                this.BeginFlush();
            }
        }

        /// <summary>
        /// Initialises a sending operation
        /// </summary>
        private void BeginFlush()
        {
            if(this.WriterQueue.TryDequeue(out IBuffer buffer))
            {
                ChannelToken channelToken = this.SendArgs.UserToken as ChannelToken;

                channelToken.DataWriter.Data = buffer.Get();
                channelToken.DataWriter.DataRemaining = buffer.GetLength();

                this.ServerSocket.StartFlush(this.SendArgs);
            }
            else
            {
                Interlocked.Exchange(ref WriterCount, 0);
            }
        }

        /// <summary>
        /// Executes when the send operation is complete, will try to flush anything else left in 
        /// write queue.
        /// </summary>
        public void OnFlushComplete()
        {
            this.Socket.Shutdown(SocketShutdown.Both);
            this.BeginFlush();
        }

        /// <summary>
        /// Gets the IP address of the current connected channel
        /// </summary>
        /// <returns>The IP address of the channel</returns>
        protected override string GetIpAddress()
        {
            return this.Socket.RemoteEndPoint.ToString().Split(':')[0];
        }
    }
}
