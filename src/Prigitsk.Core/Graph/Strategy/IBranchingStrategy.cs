using System;
using System.Collections.Generic;

namespace Prigitsk.Core.Graph.Strategy
{
    public interface IBranchingStrategy
    {
        string GetHtmlColorFor(OriginBranch branch);

        IEnumerable<OriginBranch> SortByPriority(IEnumerable<OriginBranch> branchesEnumerable);

        IEnumerable<OriginBranch> SortForWriting(
            IEnumerable<OriginBranch> branchesEnumerable,
            Dictionary<OriginBranch, DateTime> firstNodeDates);
    }
}