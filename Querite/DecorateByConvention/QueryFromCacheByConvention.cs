namespace querite
{
    internal class QueryFromCacheByConvention : DecorateQueryBy<IQueryFromCacheConvention>
    {
        private readonly CacheStoreFactory _cacheStoreFactory;

        public QueryFromCacheByConvention(IQueryFromCacheConvention convention, CacheStoreFactory cacheStoreFactory)
            : base(convention)
        {
            _cacheStoreFactory = cacheStoreFactory;
        }

        protected override IAmQuery<TModel, TSource> OnDecorate<TModel, TSource>(IAmQuery<TModel, TSource> query)
        {
            var cache = _cacheStoreFactory.Build<IAmQuery<TModel, TSource>, TModel>();
            return new QueryFromCacheDecorator<TModel, TSource>(query, cache);
        }
    }
}