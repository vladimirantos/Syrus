using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Syrus.Core.Caching
{
    internal abstract class CacheBase<K> : IDisposable
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
    }

    internal class KeyValueCache<K, T> : CacheBase<K>
    {
        private Dictionary<K, T> _cache = new Dictionary<K, T>();

        public T this[K key] => Get(key);

        /// <summary>
        /// Add item to cache. When timeout is up, this item is deleted. When cache contains this key, nothing happens.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout">Timeout in seconds</param>
        public void Add(K key, T value, int timeout = Timeout.Infinite)
        {
            if (Disposed)
                return;
            if (timeout < 1 && timeout != Timeout.Infinite)
                throw new ArgumentOutOfRangeException("Timeout must be greather than zero.");
            locker.EnterWriteLock();
            try
            {
                CheckTimer(key, timeout);
                if (!_cache.ContainsKey(key))
                    _cache.Add(key, value);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Update value in the specified key. When cache does not contains key, throw ArgumentException.
        /// </summary>
        public void Update(K key, T value)
        {
            if (Disposed)
                return;
            locker.EnterWriteLock();
            try
            {
                if (!_cache.ContainsKey(key))
                    throw new ArgumentException($"Cache not contains the specified key: {key}");
                _cache[key] = value;
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public void AddOrUpdate(K key, T value, int timeout = Timeout.Infinite)
        {
            if (Disposed)
                return;
            if (timeout < 1 && timeout != Timeout.Infinite)
                throw new ArgumentOutOfRangeException("Timeout must be greather than zero.");

            locker.EnterWriteLock();
            try
            {
                CheckTimer(key, timeout);
                if (!_cache.ContainsKey(key))
                    _cache.Add(key, value);
                else
                    _cache[key] = value;
            }
            finally
            {
                locker.ExitWriteLock();

            }
        }

        /// <summary>
        /// Get item with the specified key or return <c>default(T)</c>.
        /// </summary>
        public T Get(K key)
        {
            if (Disposed)
                return default(T);
            locker.EnterReadLock();
            try
            {
                T value;
                return _cache.TryGetValue(key, out value) ? value : default(T);
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        /// <summary>
        /// Tries to gets the cache value with the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(K key, out T value)
        {
            if (Disposed)
            {
                value = default(T);
                return false;
            }
            locker.EnterReadLock();
            try
            {
                return _cache.TryGetValue(key, out value);
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        /// <summary>
        /// Remove item by key.
        /// </summary>
        public override void Remove(K key)
        {
            if (Disposed)
                return;
            locker.EnterWriteLock();
            try
            {
                if(_cache.ContainsKey(key))
                {
                    try
                    {
                        timers[key].Dispose();
                    }
                    catch { }
                    timers.Remove(key);
                    _cache.Remove(key);
                }
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Remove item by key pattern.
        /// </summary>
        public void Remove(Predicate<K> keyPattern)
        {
            if (Disposed)
                return;
            locker.EnterWriteLock();
            try
            {
                var keysForRemove = _cache.Keys.Where(key => keyPattern(key));
                foreach(K key in keysForRemove)
                {
                    try
                    {
                        timers[key].Dispose();
                    }
                    catch { }
                    timers.Remove(key);
                    _cache.Remove(key);
                }
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Check when the specified key is in cache.
        /// </summary>
        public override bool Exists(K key)
        {
            if (Disposed)
                return false;
            locker.EnterReadLock();
            try
            {
                return _cache.ContainsKey(key);
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        /// <summary>
        /// Clear all items.
        /// </summary>
        public override void Clear()
        {
            locker.EnterWriteLock();
            try
            {
                try
                {
                    foreach (Timer t in timers.Values)
                        t.Dispose();
                }
                catch { }
                
                timers.Clear();
                _cache.Clear();
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }
    }

    internal class Cache<T> : CacheBase<T> where T : class
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

        public void Add(T value)
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

        public IEnumerable<T> GetAll()
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
