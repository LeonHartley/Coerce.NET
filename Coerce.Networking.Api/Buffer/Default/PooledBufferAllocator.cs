using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Networking.Api.Buffer.Default
{
    public class PooledBufferAllocator : IBufferAllocator
    {
        private readonly int _maxAllocations;

        private byte[] _coreBuffer;
        private Stack<int> _bufferIndexPool;
        private int _currentIndex;
        private int _allocationSize;

        public PooledBufferAllocator(int maxAllocations, int allocationSize)
        {
            this._maxAllocations = maxAllocations;
            this._allocationSize = allocationSize;

            this._coreBuffer = new byte[this._maxAllocations * this._allocationSize];
            this._bufferIndexPool = new Stack<int>();
        }

        public bool Alloc(int size, out IBuffer buffer)
        {
            if(size != this._allocationSize)
            {
                buffer = null;
                return false;
            }

            if (this._bufferIndexPool.Count > 0)
            {
                buffer = new DefaultBuffer(this._coreBuffer, this._bufferIndexPool.Pop(), this._currentIndex);
                return true;
            }

            if((_coreBuffer.Length - size) < this._currentIndex)
            {
                buffer = null;
                return false;
            }

            buffer = new DefaultBuffer(this._coreBuffer, this._currentIndex, size);
            this._currentIndex += size;

            return true;
        }

        public void Dealloc(IBuffer buffer)
        {
            this._bufferIndexPool.Push((buffer as DefaultBuffer).Offset);

            buffer.Dispose(this);
        }
    }
}
