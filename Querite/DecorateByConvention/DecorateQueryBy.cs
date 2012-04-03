namespace querite
{
    internal abstract class DecorateQueryBy<TConvention>
        where TConvention : IQueryConvention
    {
        private readonly TConvention _convention;

        protected DecorateQueryBy(TConvention convention)
        {
            _convention = convention;
        }

        public IAmQuery<TModel, TSource> Decorate<TModel, TSource>(IAmQuery<TModel, TSource> query)
        {
            return _convention.Satisifes(query.GetType())
                ? OnDecorate(query)
                : query;
        }

        protected abstract IAmQuery<TModel, TSource> OnDecorate<TModel, TSource>(IAmQuery<TModel, TSource> query);
    }
}