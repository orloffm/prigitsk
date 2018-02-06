using System;
using System.Collections.Generic;
using Prigitsk.Core.Graph;

namespace Prigitsk.Core.Nodes
{
    public interface IAssumedGraph
    {
        bool AnyPointersAreSourcedFrom(INode currentNode);
        IEnumerable<INode> EnumerateAllContainedNodes();

        IEnumerable<INode> EnumerateAllLeftOvers();

        IEnumerable<INode> EnumerateLeftOversWithoutBranchesAndTags();

        Tag[] GetAllTags();

        OriginBranch[] GetCurrentBranches();

        DateTime GetFirstNodeDate(OriginBranch branch);

        INode[] GetNodesConsecutive(OriginBranch branch);

        OriginBranch[] GetOrphanedBranches();

        void RemoveNodeFromBranch(OriginBranch branch, INode node);
        void RemoveNodeFromLeftOvers(INode n);

        void SetBranchNodes(OriginBranch branch, IEnumerable<INode> consecutiveNodes);

        // Leftovers.

        void SetLeftOverNodes(IEnumerable<INode> nodes);

        void SetTags(IEnumerable<Tag> allTags);

        // Branches.

        IEnumerable<INode> EnumerateNodesUpTheBranch(INode nodeInQuestion);
        IEnumerable<INode> EnumerateNodesDownTheBranch(INode nodeInQuestion);
        OriginBranch GetBranch(INode node);
        int GetIndexOnBranch(INode child);
    }
}