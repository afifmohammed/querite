namespace Querite
{
    public interface IAmQuery<out TModel, in TSource> : IQueryStatistics
        where TSource : class
    {
        TModel Apply(TSource source);
    }
}