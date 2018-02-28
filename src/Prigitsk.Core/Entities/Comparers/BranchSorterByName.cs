using System;
using System.Collections.Generic;

namespace Prigitsk.Core.Entities.Comparers
{
    public class BranchSorterByName : IComparer<IBranch>
    {
        public int Compare(IBranch x, IBranch y)
        {
            return string.Compare(x.Label, y.Label, StringComparison.OrdinalIgnoreCase);
        }
    }
}