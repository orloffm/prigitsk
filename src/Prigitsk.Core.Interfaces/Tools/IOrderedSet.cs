using System.Collections.Generic;

namespace Prigitsk.Core.Tools
{
    /// <summary>
    ///     A set of different objects that are stored in addition order.
    /// </summary>
    public interface IOrderedSet<T>
        : ISet<T>
    {
        T First { get; }

        T Last { get; }

        /// <summary>
        ///     Adds item in the beginning.
        /// </summary>
        bool AddFirst(T value);

        /// <summary>
        ///     Adds item in the end.
        /// </summary>
        bool AddLast(T value);

        IEnumerable<T> EnumerateAfter(T item);

        IEnumerable<T> EnumerateBefore(T item);

        T PickFirst(IEnumerable<T> items);
    }
}