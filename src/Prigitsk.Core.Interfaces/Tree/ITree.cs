using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Tree
{
    public interface ITree
    {
        /// <summary>
        ///     Adds a branch with all its commits.
        /// </summary>
        void AddBranchWithCommits(IBranch branch, IEnumerable<ICommit> commitsInBranch);
        
        /// <summary>
        ///     Adds tags.
        /// </summary>
        void AddTag(ITag tags);
        
        void AddCommit(ICommit commit);

        IEnumerable<INode> Nodes { get; }

        IBranch GetContainingBranch(INode node);

        void RemoveNode(INode node);

        IEnumerable<ITag> GetPointingTags(INode node);

        IEnumerable<ITag> GetPointingBranches(INode node);
    }
}