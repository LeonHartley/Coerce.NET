using System;
using Coerce.Networking.Api.Buffer;

namespace Coerce.Networking.Core.Buffer
{
    class Buffer : IBuffer
    {
        private byte[] _buffer;
        private int _size;

        private int _writerIndex;
        private int _readerIndex;

        public Buffer(byte[] buffer, int size)
        {
            this._buffer = buffer;
            this._size = size;
        }

        public void WriteBytes(byte[] bytes)
        {
            for(int i = 0; i < bytes.Length; i++)
            {
                this._buffer[this._writerIndex++] = bytes[i];
            }
        }

        public byte[] ReadBytes(int length)
        {
            return null;
        }

        public void WriteBytes(int offset, byte[] bytes)
        {
            for(int i = 0; i < bytes.Length; i++)
            {
                this._buffer[offset++] = bytes[i];
            }
        }

        public void Dispose(IBufferAllocator allocator)
        {
            this._buffer = null;
        }
    }
}
