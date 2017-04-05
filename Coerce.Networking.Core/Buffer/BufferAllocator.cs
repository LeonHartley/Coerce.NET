using Coerce.Networking.Api.Buffer;
using System;

namespace Coerce.Networking.Core.Buffer
{
    class BufferAllocator : IBufferAllocator
    {
        public IBuffer Alloc(int size)
        {
            // todo: pool the buffers
            return new Buffer(new byte[size], size);
        }

        public void Dealloc(IBuffer buffer)
        {
            buffer.Dispose(this);
        }
    }
}
