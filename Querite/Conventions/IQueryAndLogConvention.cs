namespace querite
{
    public interface IQueryAndLogConvention : IQueryConvention {}

    public class QueryAndLogConvention : KeywordInTypeOrNamespaceOrAttrConvention, IQueryAndLogConvention
    {
        protected override string Keyword 
        {
            get { return "Log"; }
        }
    }
}