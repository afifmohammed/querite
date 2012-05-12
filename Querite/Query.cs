using System;

namespace Querite
{
    internal class Query<TSource> : IQuery<TSource>, IDisposable where TSource : class
    {
        private Func<DateTime> _now;
        private readonly CacheStoreFactory _cacheStoreFactory;
        private Func<TSource> _getSource;
        private Action<IQueryCustomizations> _setCustomizations;
        private Action<IQueryStatistics> _setStatistics;

        public Query(Func<DateTime> now, CacheStoreFactory cacheStoreFactory, Func<TSource> getSource)
        {
            _now = now;
            _cacheStoreFactory = cacheStoreFactory;
            _getSource = getSource;
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
            
            var model = customizedQuery.Apply(_getSource());
            
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

        public void Dispose()
        {
            _cacheStoreFactory.Dispose();
            _getSource = null;
            _now = null;
            _setCustomizations = null;
            _setStatistics = null;
        }
    }
}