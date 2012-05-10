using System;
using System.Linq;

namespace Querite
{
    public interface IQueriteConvention
    {
        bool Satisfies(Type t);
    }

    public class CacheToMemoryConvention : IQueriteConvention
    {
        private const string Keyword = "InMemory";

        public virtual bool Satisfies(Type t)
        {
            return t.Name.Contains(Keyword)
                || (!string.IsNullOrEmpty(t.Namespace) && t.Namespace.Contains(Keyword))
                || t.GetCustomAttributes(inherit: false).Any(a => a.GetType().Name.Contains(Keyword));
        }
    }
}