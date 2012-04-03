using System;
using System.Collections.Generic;
using System.Linq;

namespace querite
{
    internal class EveryQueryConvention : IQueryConvention
    {
        private readonly IEnumerable<IQueryConvention> _queryConventions;

        public EveryQueryConvention(IEnumerable<IQueryConvention> queryConventions)
        {
            _queryConventions = queryConventions;
        }

        public bool Satisifes(Type type)
        {
            return _queryConventions.Any(c => c.Satisifes(type));
        }
    }
}