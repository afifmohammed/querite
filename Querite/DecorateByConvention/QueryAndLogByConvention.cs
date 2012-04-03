namespace querite
{
    internal class QueryAndLogByConvention : DecorateQueryBy<IQueryAndLogConvention>
    {
        public QueryAndLogByConvention(IQueryAndLogConvention convention) : base(convention)
        {}

        protected override IAmQuery<TModel, TSource> OnDecorate<TModel, TSource>(IAmQuery<TModel, TSource> query)
        {
            return new QueryAndLogDecorator<TModel, TSource>(query);
        }
    }
}