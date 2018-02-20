using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Tree
{
    public interface ITree
    {
        /// <summary>
        ///     Adds a branch with all its commits.
        /// </summary>
        void AddBranchCommits(IBranch branch, IEnumerable<ICommit> commitsInBranch);

        /// <summary>
        ///     Adds commits that don't have any branches attached.
        /// </summary>
        void AddCommitsWithoutBranches(IEnumerable<ICommit> commits);

        /// <summary>
        ///     Adds tags.
        /// </summary>
        void AddTags(IEnumerable<ITag> tags);
    }
}