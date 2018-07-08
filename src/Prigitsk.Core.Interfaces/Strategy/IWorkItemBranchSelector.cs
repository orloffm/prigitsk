using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Strategy
{
    public interface IWorkItemBranchSelector
    {
        bool IsLesserBranch(IBranch branch);

        void PreProcessAllBranches(IEnumerable<IBranch> branches, IWorkItemSuffixRegex lesserBranchesRegex);
    }
}