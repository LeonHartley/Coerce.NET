using Coerce.Networking.Api.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Networking.Core.Sockets
{
    class DataWriter
    {
        public int DataRemaining
        {
            get; set;
        }

        public int DataProcessed
        {
            get; set;
        }

        public byte[] Data
        {
            get; set;
        }

        public DataWriter()
        {
            this.DataRemaining = 0;
            this.DataProcessed = 0;
            this.Data = null;
        }

        public void Reset()
        {
            this.DataRemaining = 0;
            this.DataProcessed = 0;

            if (this.Data != null)
            {
                Array.Clear(this.Data, 0, this.Data.Length);
                this.Data = null;
            }
        }
    }
}
