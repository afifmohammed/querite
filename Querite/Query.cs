using System;

namespace querite
{
    internal class Query<TSource> : IQuery<TSource>
    {
        protected Action<int> SetCount;
        protected readonly TSource Source;
        private readonly DecorateQueryByEveryConvention _decorateQueryByConvention;

        public Query(TSource source, DecorateQueryByEveryConvention decorateQueryByConvention)
        {
            Source = source;
            _decorateQueryByConvention = decorateQueryByConvention;
        }

        public TModel Execute<TModel>(IAmQuery<TModel, TSource> query)
        {
            var decorated = _decorateQueryByConvention.Decorate(query);
            return decorated.Count(SetCount).Apply(Source);
        }

        public IQuery<TSource> Count(Action<int> count)
        {
            SetCount = count;
            return this;
        }
    }
}