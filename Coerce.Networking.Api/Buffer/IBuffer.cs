using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Networking.Api.Buffer
{
    public interface IBuffer
    {
        IBufferAllocator Free();
    }
}
