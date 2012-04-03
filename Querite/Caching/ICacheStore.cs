using System;

namespace querite
{
    public interface ICacheStore<in TKey, TValue>
    {
        TValue Get(TKey key);
        DateTime? ExpiryFor(TKey key);
        void Add(TKey key, TValue value, DateTime expiryAt);
    }
}