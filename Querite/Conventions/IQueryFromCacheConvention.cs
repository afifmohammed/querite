namespace querite
{
    public interface IQueryFromCacheConvention : IQueryConvention {}

    public class QueryFromCacheConvention : KeywordInTypeOrNamespaceOrAttrConvention, IQueryFromCacheConvention
    {
        protected override string Keyword
        {
            get { return "Cache"; }
        }
    }
}