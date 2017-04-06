using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Networking.Api.Buffer
{
    public interface IBufferAllocator
    {
        bool Alloc(int size, out IBuffer buffer);

        void Dealloc(IBuffer buffer);
    }
}
