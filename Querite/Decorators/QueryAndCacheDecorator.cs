namespace querite
{
    internal class QueryAndCacheDecorator<TModel, TSource> : QueryDecorator<TModel, TSource>
    {
        private readonly ICacheStore<IAmQuery<TModel, TSource>, TModel> _cacheStore;
        private bool _queried;
        public QueryAndCacheDecorator(IAmQuery<TModel, TSource> query, ICacheStore<IAmQuery<TModel, TSource>, TModel> cacheStore)
            : base(query)
        {
            _cacheStore = cacheStore;
        }

        protected override void OnBeforeApply()
        {
            _queried = false;
        }

        protected override void OnApply()
        {
            base.OnApply();
            _queried = true;
        }

        protected override void OnAfterApply()
        {
            if (!_queried) return;
            _cacheStore.Add(Query, Model, StaleAsOf().ToLocalTime().LocalDateTime);
        }
    }
}