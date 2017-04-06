using System;
using System.Collections.Generic;
using System.Text;

namespace Coerce.Commons.Collections
{
    public abstract class QueuedObjectPool<T>
    {
        private Queue<T> _objectQueue;
        private Boolean _closed;
        private int _size;
        private int _maxFreeObjects;

        private Func<T> _objectFactory;
        
        protected void Initialise(int size, int maxFreeObjects, Func<T> objectFactory)
        {
            this._size = size;
            this._maxFreeObjects = maxFreeObjects;
            this._objectQueue = new Queue<T>(size);

            this._objectFactory = objectFactory;
        }

        public virtual bool Return(T value)
        {
            lock (this.Lock)
            {
                if(this._objectQueue.Count < this._maxFreeObjects && !this._closed)
                {
                    this._objectQueue.Enqueue(value);
                    return true;
                }

                return false;
            }
        }

        public T Take()
        {
            lock(this.Lock)
            {
                if(this._objectQueue.Count == 0)
                {
                    this.AllocateObjects();
                }

                return this._objectQueue.Dequeue();
            }
        }

        public void AllocateObjects()
        {
            for(int i = 0; i < _size; i++)
            {
                this._objectQueue.Enqueue(this._objectFactory.Invoke());
            }
        }

        public void Close()
        {
            lock(this.Lock)
            {
                foreach(T item in this._objectQueue)
                {
                    if(item != null)
                    {
                        this.CleanupItem(item);
                    }
                }

                this._objectQueue.Clear();
                this._closed = true;
            }
        }

        protected virtual void CleanupItem(T item)
        {
            // we don't need to do anything by default
        }

        private object Lock
        {
            get
            {
                return this._objectQueue;
            }
        }

    }
}
