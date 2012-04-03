using System;

namespace querite
{
    public class MemcachedStoreFactory : CacheStoreFactory
    {
        private readonly MembaseClientFactory _membaseClientFactory;
        private readonly Func<DateTimeOffset> _getTime;

        public MemcachedStoreFactory(
            ICacheStoreInMemoryConvention cacheStoreInMemoryConvention,
            MembaseClientFactory membaseClientFactory,
            Func<DateTimeOffset> getTime) : base(cacheStoreInMemoryConvention)
        {
            _membaseClientFactory = membaseClientFactory;
            _getTime = getTime;
        }

        protected override ICacheStore<TCriteria, TModel> OnBuild<TCriteria, TModel>()
        {
            return new MemcachedStore<TCriteria, TModel>(_getTime, _membaseClientFactory);
        }
    }
}