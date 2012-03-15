using System;
using System.Collections.Generic;

namespace querite
{
    public abstract class AbstractQuery<TModel, TSource>  : IAmQuery<TModel, TSource>
    {
        protected Action<int> SetCount { get; set; }
        public abstract IEnumerable<TModel> Apply(TSource source);
        
        public IAmQuery<TModel, TSource> Count(Action<int> count)
        {
            SetCount = count;
            return this;
        }
    }

    public interface IAmQuery<out TModel, in TSource>
    {
        IAmQuery<TModel, TSource> Count(Action<int> count);
        IEnumerable<TModel> Apply(TSource source);
    }
}
