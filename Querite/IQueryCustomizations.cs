using System;

namespace Querite
{
    public interface IQueryCustomizations : IFluentSyntax
    {
        DateTime? CanBeStaleSince { get; set; }
        TimeSpan? CacheItFor { get; set; }
    }
}