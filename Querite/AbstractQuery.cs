using System;
using System.Collections.Generic;

namespace querite
{
    public abstract class AbstractQuery<TModel, TSource>  : IAmQuery<TModel, TSource>
    {
        private ICollection<Action<int>> SetCountActions { get; set; }
        protected Func<DateTimeOffset> StaleAsOf { get; private set; }
        protected void SetCount(int count)
        {
            foreach (var setCount in SetCountActions)
                setCount(count);
        }

        protected AbstractQuery()
        {
            Stale(asOf:() => DateTimeOffset.Now);
            SetCountActions = new List<Action<int>>();
        }

        public IAmQuery<TModel, TSource> Stale(Func<DateTimeOffset> asOf)
        {
            StaleAsOf = asOf;
            return this;
        }

        public abstract TModel Apply(TSource source);
        
        public IAmQuery<TModel, TSource> Count(Action<int> count)
        {
            SetCountActions.Add(count);
            return this;
        }
    }
}