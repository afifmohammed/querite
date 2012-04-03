using System;

namespace querite
{
    public interface IQueryConvention
    {
        bool Satisifes(Type type);
    }
}