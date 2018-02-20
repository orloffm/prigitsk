using System;
using System.Collections.Generic;
using Prigitsk.Core.Nodes;

namespace Prigitsk.Core.Graph
{
    [Obsolete]
    public interface IAssumedGraph
    {
        bool AnyPointersAreSourcedFrom(INode currentNode);

        IEnumerable<INode> EnumerateAllContainedNodes();

        IEnumerable<INode> EnumerateAllLeftOvers();

        IEnumerable<INode> EnumerateLeftOversWithoutBranchesAndTags();

        IEnumerable<INode> EnumerateNodesDownTheBranch(INode nodeInQuestion);

        // Branches.

        IEnumerable<INode> EnumerateNodesUpTheBranch(INode nodeInQuestion);

        Tag[] GetAllTags();

        OriginBranch GetBranch(INode node);

        OriginBranch[] GetCurrentBranches();

        DateTime GetFirstNodeDate(OriginBranch branch);

        int GetIndexOnBranch(INode child);

        INode[] GetNodesConsecutive(OriginBranch branch);

        OriginBranch[] GetOrphanedBranches();

        void RemoveNodeFromBranch(OriginBranch branch, INode node);

        void RemoveNodeFromLeftOvers(INode n);

        void SetBranchNodes(OriginBranch branch, IEnumerable<INode> consecutiveNodes);

        // Leftovers.

        void SetLeftOverNodes(IEnumerable<INode> nodes);

        void SetTags(IEnumerable<Tag> allTags);
    }
}