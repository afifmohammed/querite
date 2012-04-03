using System;
using Configoo;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using Membase;

namespace querite
{
    public class MemcachedStore<TKey, TValue> : ICacheStore<TKey, TValue>
    {
        private readonly Func<DateTimeOffset> _getTime;
        private readonly MemcachedClient _myKeyValueStore;
        private readonly MemcachedClient _keysExpiryStore;

        public MemcachedStore(Func<DateTimeOffset> getTime, MembaseClientFactory membaseClientFactory)
        {
            _getTime = getTime;
            var password = A<Configured>.Value.For("CacheBucketPassword", @default: "Pa55word!");

            _myKeyValueStore = membaseClientFactory.Build(MyBucketName(), password);
            _keysExpiryStore = membaseClientFactory.Build(MyBucketName("KeyExpiryFor"), password);
        }

        private string KeyValueName
        {
            get
            {
                return "{0}For{1}".For(typeof(TValue).EnglishName(), typeof(TKey).EnglishName());
            }
        }

        private string MyBucketName(string store = "")
        {
            var appConfigKey = "{0}{1}BucketName".For(store, KeyValueName);
            var keyValue = "{0}{1}".For(store, KeyValueName);

            var bucketName = A<Configured>.Value.For(appConfigKey, @default: keyValue);
            return bucketName;
        }
        
        public TValue Get(TKey key)
        {
            var hashedKey = key.ToString().ToMd5Fingerprint();

            var value = _myKeyValueStore.Get<TValue>(hashedKey);

            return value;
        }

        public DateTime? ExpiryFor(TKey key)
        {
            var expiry = _keysExpiryStore.Get<DateTime>(key.ToString().ToMd5Fingerprint());
            return expiry;
        }

        public void Add(TKey key, TValue value, DateTime expireAt)
        {
            var hashedKey = key.ToString().ToMd5Fingerprint();

            var ticks = expireAt.Ticks - _getTime().Ticks;
            var lifeSpan = new TimeSpan(Math.Abs(ticks));
            var addedKey = _myKeyValueStore.Store(StoreMode.Set, hashedKey, value.TryToList(), lifeSpan);
            var addedExpiry = _keysExpiryStore.Store(StoreMode.Set, hashedKey, expireAt, lifeSpan);
        }
    }
}