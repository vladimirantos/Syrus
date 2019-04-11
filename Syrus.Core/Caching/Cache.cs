using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Syrus.Core.Caching
{
    internal class Cache<K, T> : IDisposable
    {
        private Dictionary<K, T> _cache = new Dictionary<K, T>();
        private Dictionary<K, Timer> _timers = new Dictionary<K, Timer>();
        private ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;
            if (disposing)
            {
                Clear();
                _locker.Dispose();
            }
        }

        /// <summary>
        /// Add item to cache. When timeout is up, this item is deleted. When cache contains this key, nothing happens.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout">Timeout in miliseconds</param>
        public void Add(K key, T value, double timeout = Timeout.Infinite)
        {
            if (_disposed)
                return;
            if (timeout < 1 && timeout != Timeout.Infinite)
                throw new ArgumentOutOfRangeException("Timeout must be greather than zero.");
            _locker.EnterWriteLock();
            try
            {
                if (!_cache.ContainsKey(key))
                    _cache.Add(key, value);
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Update value in the specified key. When cache does not contains key, throw ArgumentException.
        /// </summary>
        public void Update(K key, T value)
        {
            if (_disposed)
                return;
            _locker.EnterWriteLock();
            try
            {
                if (!_cache.ContainsKey(key))
                    throw new ArgumentException($"Cache not contains the specified key: {key}");
                _cache[key] = value;
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        public void AddOrUpdate(K key, T value, int timeout = Timeout.Infinite)
        {
            if (_disposed)
                return;
            if (timeout < 1 && timeout != Timeout.Infinite)
                throw new ArgumentOutOfRangeException("Timeout must be greather than zero.");

            _locker.EnterWriteLock();
            try
            {
                if (!_cache.ContainsKey(key))
                    _cache.Add(key, value);
                else
                    _cache[key] = value;
            }
            finally
            {
                _locker.ExitWriteLock();

            }
        }

        /// <summary>
        /// Get item with the specified key or return <c>default(T)</c>.
        /// </summary>
        public T Get(K key)
        {
            if (_disposed)
                return default(T);
            _locker.EnterReadLock();
            try
            {
                T value;
                return _cache.TryGetValue(key, out value) ? value : default(T);
            }
            finally
            {
                _locker.ExitReadLock();
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
            if (_disposed)
            {
                value = default(T);
                return false;
            }
            _locker.EnterReadLock();
            try
            {
                return _cache.TryGetValue(key, out value);
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        /// <summary>
        /// Remove item by key.
        /// </summary>
        public void Remove(K key)
        {
            if (_disposed)
                return;
            _locker.EnterWriteLock();
            try
            {
                if(_cache.ContainsKey(key))
                {
                    _cache.Remove(key);
                }
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Remove item by key pattern.
        /// </summary>
        public void Remove(Predicate<K> keyPattern)
        {
            if (_disposed)
                return;
            _locker.EnterWriteLock();
            try
            {
                var keysForRemove = _cache.Keys.Where(key => keyPattern(key));
                foreach(K key in keysForRemove)
                {
                    _cache.Remove(key);
                }
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Check when the specified key is in cache.
        /// </summary>
        public bool Exists(K key)
        {
            if (_disposed)
                return false;
            _locker.EnterReadLock();
            try
            {
                return _cache.ContainsKey(key);
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        /// <summary>
        /// Clear all items.
        /// </summary>
        public void Clear()
        {
            _locker.EnterWriteLock();
            try
            {
                _cache.Clear();
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }
    }
}
