using System;
using System.Diagnostics;

namespace Querite
{
    internal class CachedQuery<TModel, TSource> : IAmQuery<TModel, TSource>, IQueryCustomizations
        where TSource : class
    {
        private readonly Func<DateTime> _now;
        private readonly IAmQuery<TModel, TSource> _query;
        private readonly ICacheStore<IAmQuery<TModel, TSource>, TModel> _queryCache;
        private readonly ICacheStore<IAmQuery<TModel, TSource>, LastQueryStats> _statsCache;

        public CachedQuery(
            Func<DateTime> now,
            IAmQuery<TModel, TSource> query,
            ICacheStore<IAmQuery<TModel, TSource>, TModel> queryCache,
            ICacheStore<IAmQuery<TModel, TSource>, LastQueryStats> statsCache)
        {
            _now = now;
            _query = query;
            _queryCache = queryCache;
            _statsCache = statsCache;
        }

        public TModel Apply(TSource source)
        {
            Debug.Print(string.Format("Inside Apply {0}:{1}", _now().Second, _now().Millisecond));
            if (CanBeStale)
            {
                Debug.Print(string.Format("Try retreiving from Cache {0}:{1}", _now().Second, _now().Millisecond));
                var stats = _statsCache.Get(_query);
                if (stats != null && stats.LastRun >= CanBeStaleSince)
                {
                    var timer = new Stopwatch();
                    timer.Start();
                    var cache = _queryCache.Get(_query);
                    timer.Stop();

                    if (cache.IsNotNull())
                    {
                        Count = stats.Count;
                        ExecutionSpan = timer.Elapsed;
                        Debug.Print(string.Format("Return {0}:{1}", _now().Second, _now().Millisecond));
                        return cache;
                    }
                }
            }

            Debug.Print(string.Format("Applying {0}:{1}", _now().Second, _now().Millisecond));
            var model = _query.Apply(source);
            Debug.Print(string.Format("Applied {0}:{1}", _now().Second, _now().Millisecond));

            Count = _query.Count;
            ExecutionSpan = _query.ExecutionSpan;

            if(CacheItFor.HasValue)
            {
                Debug.Print(string.Format("Adding to cache {0}:{1}", _now().Second, _now().Millisecond));
                var expiry = _now().Add(CacheItFor.Value);
                _queryCache.Add(_query, model, expiry);
                _statsCache.Add(_query, new LastQueryStats { Count = this.Count, LastRun = _now()}, expiry);
                Debug.Print(string.Format("Added to cache {0}:{1}", _now().Second, _now().Millisecond));
            }

            Debug.Print(string.Format("Return {0}:{1}", _now().Second, _now().Millisecond));
            return model;
        }

        public int? Count { get; private set; }
        public TimeSpan? ExecutionSpan { get; private set; }
        public DateTime? CanBeStaleSince { get; set; }
        public TimeSpan? CacheItFor { get; set; }
        
        private bool CanBeStale { get { return CanBeStaleSince < _now(); } }
    }
}