using System.Collections.Generic;

namespace Querite
{
    public abstract class AbstractQuery<TModel, TSource>  : IAmQuery<TModel, TSource>
    {
        protected int Total { get; set; }
        public abstract IEnumerable<TModel> Query(TSource source);
        
        public IAmQuery<TModel, TSource> Count(out int count)
        {
            count = Total;
            return this;
        }
    }

    public interface IAmQuery<out TModel, in TSource>
    {
        IAmQuery<TModel, TSource> Count(out int total);
        IEnumerable<TModel> Query(TSource source);
    }
}
