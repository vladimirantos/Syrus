using System;
using System.Collections.Generic;
using System.Text;

namespace Syrus.Core.Caching
{
    internal class Cache<K, T> : IDisposable
    {
        public void Add(K key, T value)
        {

        }

        /// <summary>
        /// Add item to cache. When timeout is up, this item is deleted.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout">Timeout in miliseconds</param>
        public void Add(K key, T value, double timeout)
        {

        }

        public void Update(K key, T value)
        {

        }

        /// <summary>
        /// Get item with the specified key or return <c>default(T)</c>.
        /// </summary>
        public T Get(K key)
        {
            return default(T);
        }

        /// <summary>
        /// Tries to gets the cache value with the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(K key, out T value)
        {
            value = default(T);
            return false;
        }

        /// <summary>
        /// Remove item by key.
        /// </summary>
        public void Remove(K key)
        {

        }

        /// <summary>
        /// Remove item by key pattern.
        /// </summary>
        public void Remove(Predicate<K> keyPattern)
        {

        }

        /// <summary>
        /// Check when the specified key is in cache.
        /// </summary>
        public bool Exists(K key)
        {
            return false;
        }

        /// <summary>
        /// Clear all items.
        /// </summary>
        public void Clear()
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
