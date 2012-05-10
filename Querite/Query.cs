using System;

namespace Querite
{
    internal class Query<TSource> : IQuery<TSource> where TSource  : class
    {
        private readonly Func<DateTime> _now;
        private readonly CacheStoreFactory _cacheStoreFactory;
        private readonly TSource _source;
        private Action<IQueryCustomizations> _setCustomizations;
        private Action<IQueryStatistics> _setStatistics;

        public Query(Func<DateTime> now, CacheStoreFactory cacheStoreFactory, TSource source)
        {
            _now = now;
            _cacheStoreFactory = cacheStoreFactory;
            _source = source;
            _setCustomizations = c => { };
            _setStatistics = s => { };
        }

        public TModel Execute<TModel>(IAmQuery<TModel, TSource> query)
        {
            var customizedQuery = new CachedQuery<TModel, TSource>(
                now: _now,
                query: query,
                queryCache: _cacheStoreFactory.Build<IAmQuery<TModel, TSource>, TModel>(),
                statsCache: _cacheStoreFactory.Build<IAmQuery<TModel, TSource>, LastQueryStats>());

            _setCustomizations(customizedQuery);
            
            var model = customizedQuery.Apply(_source);
            
            _setStatistics(customizedQuery);
            
            return model;
        }

        public IQuery<TSource> Customize(Action<IQueryCustomizations> customizationAction)
        {
            _setCustomizations += customizationAction;
            return this;
        }

        public IQuery<TSource> Statistics(Action<IQueryStatistics> statisticsAction)
        {
            _setStatistics += statisticsAction;
            return this;
        }
    }
}