namespace querite
{
    internal class QueryFromCacheDecorator<TModel, TSource> : QueryDecorator<TModel, TSource>
    {
        private readonly ICacheStore<IAmQuery<TModel, TSource>, TModel> _cacheStore;
        private bool NotInCache { get { return !_cacheStore.ExpiryFor(Query).HasValue; } }

        public QueryFromCacheDecorator(IAmQuery<TModel, TSource> query, ICacheStore<IAmQuery<TModel, TSource>, TModel> cacheStore)
            : base(query)
        {
            _cacheStore = cacheStore;
        }

        protected override void OnBeforeApply()
        {
            if (NotInCache) return;

            Model = _cacheStore.Get(Query);
        }

        protected override void OnApply()
        {
            if(NotInCache)
                base.OnApply();
        }
    }
}