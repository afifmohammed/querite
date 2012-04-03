namespace querite
{
    internal class QueryDecorator<TModel, TSource> : AbstractQuery<TModel, TSource>
    {
        protected readonly IAmQuery<TModel, TSource> Query;
        protected TModel Model;
        private TSource _source;
        protected int? TotalRecordsReturned { get; private set; }

        public QueryDecorator(IAmQuery<TModel, TSource> query)
        {
            Query = query;
        }

        protected virtual void OnBeforeApply() {}
        protected virtual void OnAfterApply() { }
        protected virtual void OnApply()
        {
            Model = Query.Count(x => TotalRecordsReturned = x).Apply(_source);
        }

        public override TModel Apply(TSource source)
        {
            _source = source;
            OnBeforeApply();
            OnApply();
            OnAfterApply();
            return Model;
        }
    }
}