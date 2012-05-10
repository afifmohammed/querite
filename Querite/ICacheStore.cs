using System;
using System.Collections.Generic;

namespace Querite
{
    public interface ICacheStore<in TKey, TValue>
    {
        TValue Get(TKey key);
        void Add(TKey key, TValue value, DateTime expiryAt);
    }

    internal sealed class InMemoryCache<TKey, TValue> : ICacheStore<TKey, TValue>, IDisposable
    {
        private readonly IDictionary<string, TValue> _cache = new Dictionary<string, TValue>();
        private readonly IDictionary<string, DateTime> _keyExpiryCache = new Dictionary<string, DateTime>();

        public TValue Get(TKey key)
        {
            if (DateTime.Now > ExpiryFor(key)) return default(TValue);

            TValue value;
            var hashedKey = key.ToString().ToMd5Fingerprint();

            _cache.TryGetValue(hashedKey, out value);

            return value;
        }

        public DateTime? ExpiryFor(TKey key)
        {
            DateTime value;
            return _keyExpiryCache.TryGetValue(key.ToString().ToMd5Fingerprint(), out value) ? value : default(DateTime?);
        }

        public void Add(TKey key, TValue value, DateTime expiresAt)
        {
            var hashedKey = key.ToString().ToMd5Fingerprint();
            value = value.TryToList();

            // if we already have the key but it has expired, update the values.
            if (_cache.ContainsKey(hashedKey) && DateTime.Now > ExpiryFor(key))
            {
                _cache[hashedKey] = value;
                _keyExpiryCache[hashedKey] = expiresAt;
            }
            else
            {
                _cache.Add(hashedKey, value);
                _keyExpiryCache.Add(hashedKey, expiresAt);
            }

        }

        public void Dispose()
        {
            _cache.Clear();
            _keyExpiryCache.Clear();
        }
    }
}