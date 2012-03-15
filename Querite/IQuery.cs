using System;
using System.Collections.Generic;
using Ninject;

namespace querite
{
    public interface IQuery<out TSource>
    {
        IEnumerable<TModel> Execute<TModel>(IAmQuery<TModel, TSource> query);
        IQuery<TSource> Count(Action<int> count);
    }

    internal class Query<TSource> : IQuery<TSource>
    {
        protected Action<int> SetCount;
        [Inject] protected TSource Source { get; set; }

        public IEnumerable<TModel> Execute<TModel>(IAmQuery<TModel, TSource> query)
        {
            return query.Count(SetCount).Apply(Source);
        }

        public IQuery<TSource> Count(Action<int> count)
        {
            SetCount = count;
            return this;
        }
    }
}