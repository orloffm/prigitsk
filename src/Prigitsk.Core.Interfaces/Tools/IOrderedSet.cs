using System.Collections.Generic;

namespace Prigitsk.Core.Tools
{
    public interface IOrderedSet<T>
        : ICollection<T>
    {
        T First { get; }

        T Last { get; }
    }
}