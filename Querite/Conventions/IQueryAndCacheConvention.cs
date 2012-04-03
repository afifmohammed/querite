namespace querite
{
    public interface IQueryAndCacheConvention : IQueryConvention {}

    public class QueryAndCacheConvention : KeywordInTypeOrNamespaceOrAttrConvention, IQueryAndCacheConvention
    {
        protected override string Keyword
        {
            get { return "Cache"; }
        }
    }
}