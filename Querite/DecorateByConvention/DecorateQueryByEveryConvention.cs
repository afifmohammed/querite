namespace querite
{
    internal class DecorateQueryByEveryConvention : DecorateQueryBy<EveryQueryConvention>
    {
        private readonly QueryAndLogByConvention _queryAndLogByConvention;
        private readonly QueryAndCacheByConvention _queryAndCacheByConvention;
        private readonly QueryFromCacheByConvention _queryFromCacheByConvention;

        public DecorateQueryByEveryConvention(
            QueryAndLogByConvention queryAndLogByConvention, 
            QueryAndCacheByConvention queryAndCacheByConvention,
            QueryFromCacheByConvention queryFromCacheByConvention, 
            EveryQueryConvention everyQueryConvention) : base(everyQueryConvention)
        {
            _queryAndLogByConvention = queryAndLogByConvention;
            _queryAndCacheByConvention = queryAndCacheByConvention;
            _queryFromCacheByConvention = queryFromCacheByConvention;
        }

        protected override IAmQuery<TModel, TSource> OnDecorate<TModel, TSource>(IAmQuery<TModel, TSource> query)
        {
            query = _queryAndCacheByConvention.Decorate(query);
            query = _queryFromCacheByConvention.Decorate(query);
            query = _queryAndLogByConvention.Decorate(query);
            return query;
        }
    }
}