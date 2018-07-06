using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Rendering
{
    public interface ILesserBranchSelector
    {
        bool IsLesserBranch(IBranch branch);

        void PreProcessAllBranches(IEnumerable<IBranch> branches, string lesserBranchesRegex);
    }
}