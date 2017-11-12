using System;
using System.Collections.Generic;

namespace GitWriter.Core.Graph.Strategy
{
    public class BranchSorterByDate : IComparer<OriginBranch>
    {
        private readonly Dictionary<OriginBranch, DateTime> _startDates;

        public BranchSorterByDate(Dictionary<OriginBranch, DateTime> startDates)
        {
            _startDates = startDates;
        }

        public int Compare(OriginBranch x, OriginBranch y)
        {
            DateTime xd = _startDates[x];
            DateTime yd = _startDates[y];

            return xd.CompareTo(yd);
        }
    }
}