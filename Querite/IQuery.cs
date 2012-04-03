using System;

namespace querite
{
    public interface IQuery<out TSource>
    {
        TModel Execute<TModel>(IAmQuery<TModel, TSource> query);
        IQuery<TSource> Count(Action<int> count);
    }
}