using Coerce.Networking.Api.Channels;
using System;
using System.Collections.Generic;
using System.Text;
using Coerce.Networking.Api.Buffer;

namespace Coerce.Networking.Core.Channels
{
    class CoreChannel : Channel
    {
        /// <summary>
        /// Ensures there is only 1 write operation executed at a time
        /// </summary>
        private int WriterCount { get; set; }

        /// <summary>
        /// The queue of buffers to write to the client, if there's currently an on-going write operation,
        /// buffers will be queued until the next write operation is initiated.
        /// </summary>
        private Queue<IBuffer> WriterQueue { get; set; }

        /// <summary>
        /// Writes to the channel
        /// </summary>
        /// <param name="buffer">The buffer in which to write to the channel</param>
        public override void Write(IBuffer buffer)
        {
            this.WriterQueue.Enqueue(buffer);
        }

        /// <summary>
        /// Gets the IP address of the current connected channel
        /// </summary>
        /// <returns>The IP address of the channel</returns>
        protected override string GetIpAddress()
        {
            return "127.0.0.1";
        }
    }
}
