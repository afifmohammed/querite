namespace querite
{
    public abstract class CacheStoreFactory
    {
        private readonly ICacheStoreInMemoryConvention _cacheStoreInMemoryConvention;

        public CacheStoreFactory(ICacheStoreInMemoryConvention cacheStoreInMemoryConvention)
        {
            _cacheStoreInMemoryConvention = cacheStoreInMemoryConvention;
        }

        public ICacheStore<TCriteria, TModel> Build<TCriteria, TModel>()
        {
            return _cacheStoreInMemoryConvention.Satisifes(typeof (TCriteria))
                       ? new InMemoryCache<TCriteria, TModel>()
                       : OnBuild<TCriteria, TModel>();
        }

        protected abstract ICacheStore<TCriteria, TModel> OnBuild<TCriteria, TModel>();
    }
}