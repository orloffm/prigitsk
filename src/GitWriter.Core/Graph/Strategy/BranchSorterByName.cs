﻿using System;
using System.Collections.Generic;

namespace GitWriter.Core.Graph.Strategy
{
    public class BranchSorterByName : IComparer<OriginBranch>
    {
        public int Compare(OriginBranch x, OriginBranch y)
        {
            return string.Compare(x.Label, y.Label, StringComparison.OrdinalIgnoreCase);
        }
    }
}