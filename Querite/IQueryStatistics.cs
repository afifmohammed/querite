using System;

namespace Querite
{
    public interface IQueryStatistics : IFluentSyntax
    {
        int? Count { get; }
        TimeSpan? ExecutionSpan { get; }
    }
}