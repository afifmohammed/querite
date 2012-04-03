namespace querite
{
    internal class QueryAndCacheByConvention : DecorateQueryBy<IQueryAndCacheConvention>
    {
        private readonly CacheStoreFactory _cacheStoreFactory;

        public QueryAndCacheByConvention(IQueryAndCacheConvention convention, CacheStoreFactory cacheStoreFactory) : base(convention)
        {
            _cacheStoreFactory = cacheStoreFactory;
        }

        protected override IAmQuery<TModel, TSource> OnDecorate<TModel, TSource>(IAmQuery<TModel, TSource> query)
        {
            var cacheStore = _cacheStoreFactory.Build<IAmQuery<TModel, TSource>, TModel>();
            return new QueryAndCacheDecorator<TModel, TSource>(query, cacheStore);
        }
    }
}