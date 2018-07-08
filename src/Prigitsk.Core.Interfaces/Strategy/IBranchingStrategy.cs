using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Strategy
{
    /// <summary>
    ///     The branching strategy used in the repository.
    /// </summary>
    public interface IBranchingStrategy
    {
        /// <summary>
        ///     Creates a branches knowledge object that contains the information
        ///     about the branches in the repository.
        /// </summary>
        IBranchesKnowledge CreateKnowledge(IEnumerable<IBranch> branches);
    }
}