using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Syrus.Core.Caching
{
    internal class Cache<T> : CacheBase<T> where T : class
    {
        private List<T> _cache = new List<T>();

        public List<T> Items => _cache;

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

        public override string JsonSerialize() => JsonConvert.SerializeObject(_cache, Formatting.Indented);


        public override void Deserialize(string json)
        {
            
        }
    }

    internal class CacheFacade<T> where T: class
    {
        protected Cache<T> _cache;
        protected string _location;

        public CacheFacade(string location)
        {
            _location = location;
            _cache = new Cache<T>();
        }

        public virtual void Add(T item) => _cache.Add(item);

        public virtual void Remove(T item) => _cache.Remove(item);

        public virtual bool Exists(T item) => _cache.Exists(item);

        public virtual void Save() => File.WriteAllText(_location, SerializeCache());

        public virtual string SerializeCache() => _cache.JsonSerialize();
    }
}
