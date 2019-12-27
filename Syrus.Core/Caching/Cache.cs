using Newtonsoft.Json;
using Syrus.Shared.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Syrus.Core.Caching
{
    internal class Cache<T> : CacheBase<T>, IEnumerable<T> where T : class
    {
        private readonly IStorage<T> _storage;
        private List<T> Items { get; set; } = new List<T>();

        public Cache(IStorage<T> storage) => _storage = storage;

        public override void Clear()
        {
            locker.EnterWriteLock();
            try
            {
                Items.Clear();
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
                return Items.Any(item => item == value);
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
                Items.Remove(key);
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
                Items.Add(value);
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
                return Items;
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        public IEnumerator<T> GetEnumerator() => GetAll().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetAll().GetEnumerator();

        public override void Load()
        {
            if (Disposed)
                return;
            locker.EnterWriteLock();
            try
            {
                Items = _storage.GetAll().ToList();
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public override void Save()
        {
            if (Disposed)
                return;
            locker.EnterReadLock();
            try
            {
                _storage.Save(Items);
            }
            finally
            {
                locker.ExitReadLock();
            }
        }
    }

    public class CacheFacade<T> where T: class
    {
        private Cache<T> _cache;
        protected string _location;
        public CacheFacade(string location)
        {
            _location = location;
            _cache = new Cache<T>(new JsonStorage<T>(location));
        }

        public virtual void Add(T item) => _cache.Add(item);

        public virtual void Remove(T item) => _cache.Remove(item);

        public virtual bool Exists(T item) => _cache.Exists(item);

        public virtual void Save() => _cache.Save();

        public virtual void Load() => _cache.Load();
    }
}
