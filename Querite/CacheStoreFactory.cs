using System;

namespace Querite
{
    internal class CacheStoreFactory : IDisposable
    {
        private Func<Type, object> _resolver;

        public CacheStoreFactory(Func<Type, object> resolver)
        {
            _resolver = resolver;
        }

        public ICacheStore<TKey, TValue> Build<TKey, TValue>()
        {
            return _resolver(typeof (ICacheStore<TKey, TValue>)) as ICacheStore<TKey, TValue>;
        }

        public void Dispose()
        {
            _resolver = null;
        }
    }
}