using System;
using System.Collections.Generic;

namespace Prigitsk.Framework
{
    public interface IPairList<T, TU>
    {
        void Add(T key, TU value);

        void Clear();

        bool Contains(T key, TU value);

        IEnumerable<Tuple<T, TU>> EnumerateItems();
    }
}