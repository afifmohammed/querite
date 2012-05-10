using System;

namespace Querite
{
    public interface IQuery<out TSource> : IFluentSyntax
        where TSource : class
    {
        TModel Execute<TModel>(IAmQuery<TModel, TSource> query);
        IQuery<TSource> Customize(Action<IQueryCustomizations> customizationAction);
        IQuery<TSource> Statistics(Action<IQueryStatistics> statisticsAction);
    }
}