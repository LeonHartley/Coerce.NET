using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Networking.Api.Buffer.Default
{
    public class DefaultBuffer : IBuffer
    {
        private byte[] _buffer;
        private int _size;

        private readonly int _offset;

        private int _writerIndex;
        private int _readerIndex;

        public DefaultBuffer(byte[] buffer, int size, int offset)
        {
            this._buffer = buffer;
            this._size = size;
        }

        public void WriteBytes(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                this._buffer[this._offset + this._writerIndex++] = bytes[i];
            }
        }

        public byte[] ReadBytes(int length)
        {
            return null;
        }

        public void WriteBytes(int offset, byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                this._buffer[this._offset + offset++] = bytes[i];
            }
        }

        public void Dispose(IBufferAllocator allocator)
        {
            this._buffer = null;
        }

        public byte[] Get()
        {
            return this._buffer;
        }

        public int GetLength()
        {
            return this._writerIndex;
        }

        public bool IsReadable()
        {
            return this._readerIndex < this._size;
        }

        public int Offset
        {
            get
            {
                return this._offset;
            }
        }
    }
}
