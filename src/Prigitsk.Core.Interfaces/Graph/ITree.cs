using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Graph
{
    public interface ITree
    {
        IEnumerable<IBranch> Branches { get; }

        IEnumerable<INode> Nodes { get; }

        IEnumerable<ITag> Tags { get; }

        /// <summary>
        ///     Adds a branch with all its commits.
        /// </summary>
        void AddBranch(IBranch branch, IEnumerable<IHash> hashesInBranch);

        void AddCommit(ICommit commit);

        /// <summary>
        ///     Adds tags.
        /// </summary>
        void AddTag(ITag tags);

        void DropTag(ITag tag);

        IEnumerable<INode> EnumerateNodes(IBranch branch);

        IEnumerable<INode> EnumerateNodesDownTheBranch(INode node);

        IEnumerable<INode> EnumerateNodesUpTheBranch(INode node);

        INode FindOldestItemOnBranch(IEnumerable<INode> allAmChildrenOnB);

        INode GetBranchTip(IBranch branch);

        IBranch GetContainingBranch(INode node);

        /// <summary>
        ///     Returns node by hash or null if it does not exist.
        /// </summary>
        INode GetNode(IHash ihash);

        IEnumerable<IBranch> GetPointingBranches(INode node);

        IEnumerable<ITag> GetPointingTags(INode node);

        bool IsStartingNodeOfBranch(INode node);

        /// <summary>
        ///     Removes the edge. Throws if this is the main connection between them.
        /// </summary>
        void RemoveEdge(INode parent, INode child);

        void RemoveNode(INode node);
    }
}