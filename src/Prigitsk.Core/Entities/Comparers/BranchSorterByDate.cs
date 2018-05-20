using System;
using System.Collections.Generic;
using System.Linq;

namespace Prigitsk.Core.Entities.Comparers
{
    public class BranchSorterByDate : IComparer<IBranch>
    {
        private readonly Dictionary<IBranch, DateTimeOffset?> _startDates;

        public BranchSorterByDate(IDictionary<IBranch, DateTimeOffset?> startDates)
        {
            _startDates = startDates.ToDictionary(k => k.Key, k => k.Value);
        }

        public int Compare(IBranch x, IBranch y)
        {
            DateTimeOffset xd = _startDates[x] ?? DateTimeOffset.MaxValue;
            DateTimeOffset yd = _startDates[y] ?? DateTimeOffset.MaxValue;
            
            return xd.CompareTo(yd);
        }
    }
}