using System;
using System.Diagnostics;

namespace Querite
{
    public abstract class AbstractQuery<TModel, TSource> : IAmQuery<TModel, TSource>
        where TSource : class
    {
        public TModel Apply(TSource source)
        {
            var timer = new Stopwatch();
            timer.Start();
            var model = OnApply(source);
            timer.Stop();
            ExecutionSpan = timer.Elapsed;
            return model;
        }

        protected abstract TModel OnApply(TSource source);
        public int? Count { get; protected set; }
        public TimeSpan? ExecutionSpan { get; private set; }
    }
}