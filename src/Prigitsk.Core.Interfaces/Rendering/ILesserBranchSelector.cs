using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Rendering
{
    public interface ILesserBranchSelector
    {
        void PreProcessAllBranches(IEnumerable<IBranch> branches, string lesserBranchesRegex);

        bool IsLesserBranch(IBranch branch);
    }
}