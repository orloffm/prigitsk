using System.Collections.Generic;
using OrlovMikhail.GitTools.Common;

namespace OrlovMikhail.GitTools.Structure
{
    public interface IBranchingStrategy
    {
        /// <summary>Orders branch names by priority according to the branching strategy.</summary>
        IEnumerable<string> OrderBranchNames(IEnumerable<string> branches);

        Color GetColorForBranch(string branchName);
    }
}