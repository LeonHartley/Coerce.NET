using System;
using System.Collections.Generic;
using System.Text;
using Coerce.Networking.Api.Buffer;

namespace Coerce.Networking.Api.Buffer.Default
{
    public class UnpooledBufferAllocator : IBufferAllocator
    {
        public static UnpooledBufferAllocator Instance = new UnpooledBufferAllocator();

        public bool Alloc(int size, out IBuffer buffer)
        {
            buffer = new DefaultBuffer(new byte[size], size, 0);
            return true;
        }

        public void Dealloc(IBuffer buffer)
        {
            // Deallocate the buffer, since we're an unpooled allocator we don't really need to do much,
            // other than to dispose the buffer
            buffer.Dispose(this);
        }
    }
}
