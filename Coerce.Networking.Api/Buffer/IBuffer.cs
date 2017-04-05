using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Networking.Api.Buffer
{
    public interface IBuffer
    {
        void WriteBytes(byte[] bytes);

        void Dispose(IBufferAllocator allocator);

        byte[] Get();
    }
}
