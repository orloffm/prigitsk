﻿using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Tree
{
    public interface ITree
    {
        IEnumerable<IBranch> Branches { get; }
        IEnumerable<ITag> Tags { get; }

        IEnumerable<INode> Nodes { get; }

        /// <summary>
        ///     Adds a branch with all its commits.
        /// </summary>
        void AddBranchWithCommits(IBranch branch, IEnumerable<ICommit> commitsInBranch);

        void AddCommit(ICommit commit);

        /// <summary>
        ///     Adds tags.
        /// </summary>
        void AddTag(ITag tags);

        void DropTag(ITag tag);

        IEnumerable<INode> EnumerateNodesDownTheBranch(INode node);

        IEnumerable<INode> EnumerateNodesUpTheBranch(INode node);

        INode FindOldestItemOnBranch(IEnumerable<INode> allAmChildrenOnB);

        IEnumerable<INode> GetAllBranchNodes(IBranch branch);

        INode GetBranchTip(IBranch branch);

        IBranch GetContainingBranch(INode node);

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