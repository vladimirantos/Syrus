using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Syrus.Core.Caching
{
    internal class Cache<T> : CacheBase<T>, ICacheDataProvider<IEnumerable<T>> where T : class
    {
        private List<T> _cache = new List<T>();

        public override void Clear()
        {
            locker.EnterWriteLock();
            try
            {
                _cache.Clear();
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public override bool Exists(T value)
        {
            if (Disposed)
                return false;
            locker.EnterReadLock();
            try
            {
                return _cache.Any(item => item == value);
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        public override void Remove(T key)
        {
            if (Disposed)
                return;
            locker.EnterWriteLock();
            try
            {
                _cache.Remove(key);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public virtual void Add(T value)
        {
            if (Disposed)
                return;
            locker.EnterWriteLock();
            try
            {
                _cache.Add(value);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public IEnumerable<T> GetValues()
        {
            if (Disposed)
                return new List<T>();
            locker.EnterReadLock();
            try
            {
                return _cache;
            }
            finally
            {
                locker.ExitReadLock();
            }
        }
    }
}
