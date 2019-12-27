using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Syrus.Core.Caching
{
    public abstract class CacheBase<K> : IDisposable
    {
        protected readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        protected readonly Dictionary<K, Timer> timers = new Dictionary<K, Timer>();

        protected bool Disposed { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;
            Disposed = true;
            if (disposing)
            {
                Clear();
                locker.Dispose();
            }
        }

        public abstract void Clear();
        public abstract void Remove(K key);
        public abstract bool Exists(K key);

        protected void CheckTimer(K key, int timeout)
        {
            void TimerRemove(object state)
            {
                Remove((K)state);
            }
            timers.Add(key, new Timer(new TimerCallback(TimerRemove), key,
                        timeout == Timeout.Infinite ? Timeout.Infinite : timeout * 1000, Timeout.Infinite));
        }
        public abstract void Save();
        public abstract void Load();
    }
}
