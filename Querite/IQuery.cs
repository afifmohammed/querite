using System.Collections.Generic;

namespace Querite
{
    public interface IQuery<out TSource>
    {
        IEnumerable<TModel> Execute<TModel>(IAmQuery<TModel, TSource> query);
        IQuery<TSource> Count(out int count);
    }

    public class Query<TSource> : IQuery<TSource>
    {
        protected int Total;
        protected TSource Source { get; set; }

        public IEnumerable<TModel> Execute<TModel>(IAmQuery<TModel, TSource> query)
        {
            return query.Count(out Total).Query(Source);
        }

        public IQuery<TSource> Count(out int count)
        {
            count = Total;
            return this;
        }
    }
}