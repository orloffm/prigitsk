using System.Collections.Generic;

namespace Prigitsk.Core.Tools
{
    public interface IOrderedSet<T>
        : ISet<T>
    {
        T First { get; }

        T Last { get; }
    }
}