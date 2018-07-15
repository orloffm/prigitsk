using System;
using System.Collections.Generic;

namespace Prigitsk.Framework
{
    public static class PairListExtensions
    {
        public static PairList<TA, TB> ToPairList<TA, TB>(this IEnumerable<Tuple<TA, TB>> source)
        {
            return new PairList<TA, TB>(source);
        }
    }
}