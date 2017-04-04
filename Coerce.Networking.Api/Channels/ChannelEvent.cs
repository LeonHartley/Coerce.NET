using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Networking.Api.Channels
{
    public abstract class ChannelEvent
    {
        private Boolean _isComplete = false;

        public Boolean IsComplete
        {
            get
            {
                return this._isComplete;
            }
        }

        public void SetCompleted(Boolean completed)
        {
            this._isComplete = completed;
        }
    }
}
