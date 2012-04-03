using System;
using System.Linq;

namespace querite
{
    public abstract class KeywordInTypeOrNamespaceOrAttrConvention : IQueryConvention
    {
        protected abstract string Keyword {get;}
        public bool Satisifes(Type type)
        {
            return type.Name.Contains(Keyword)
                   || (!string.IsNullOrEmpty(type.Namespace) && type.Namespace.Contains(Keyword)
                       || type.GetCustomAttributes(inherit: false).Any(a => a.GetType().Name.Contains(Keyword)));
        }
    }
}