using System.Collections.Generic;

namespace Prigitsk.Core.Tools
{
    public interface IOrderedSet<T>
        : ISet<T>
    {
        T First { get; }

        T Last { get; }

        /// <summary>
        ///     Adds item in the beginning.
        /// </summary>
        void AddFirst(T value);

        /// <summary>
        ///     Adds item in the end.
        /// </summary>
        void AddLast(T value);
    }
}