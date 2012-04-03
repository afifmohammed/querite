namespace querite
{
    public interface ICacheStoreInMemoryConvention : IQueryConvention
    {}

    public class CacheStoreInMemoryConvention : KeywordInTypeOrNamespaceOrAttrConvention, ICacheStoreInMemoryConvention
    {
        protected override string Keyword
        {
            get { return "CacheInMemory"; }
        }
    }
}