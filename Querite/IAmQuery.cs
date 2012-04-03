using System;

namespace querite
{
    public interface IAmQuery<out TModel, in TSource>
    {
        IAmQuery<TModel, TSource> Count(Action<int> count);
        IAmQuery<TModel, TSource> Stale(Func<DateTimeOffset> asOf);
        TModel Apply(TSource source);
    }
}
