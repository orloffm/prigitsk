using System;
using System.Collections.Generic;

namespace Prigitsk.Framework
{
    /// <summary>
    ///     The list that holds two items.
    /// </summary>
    public interface IPairList<TA, TB> : IList<Tuple<TA, TB>>
    {
        void Add(TA a, TB b);

        bool Contains(TA a, TB b);
    }
}