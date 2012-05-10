using System;
using System.Collections.Generic;

namespace Querite
{
    class QueryUsage
    {
        void Demo<TSource>(IQuery<TSource> query) where TSource : class
        {
            TimeSpan? howlong;
            int? count;
            query.Customize(x => x.CacheItFor = 10.Minutes())
                .Customize(x => x.CanBeStaleSince = 10.MinutesFrom(DateTime.Now))
                .Statistics(x => howlong = x.ExecutionSpan)
                .Statistics(x => count = x.Count)
                .Execute<IEnumerable<string>>(null);
        }
    }
}