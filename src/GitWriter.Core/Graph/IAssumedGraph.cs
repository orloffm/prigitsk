using System;
using System.Collections.Generic;
using GitWriter.Core.Graph;

namespace GitWriter.Core.Nodes
{
    public interface IAssumedGraph
    {
        bool AnyPointersAreSourcedFrom(Node currentNode);
        IEnumerable<Node> EnumerateAllContainedNodes();

        IEnumerable<Node> EnumerateAllLeftOvers();

        IEnumerable<Node> EnumerateLeftOversWithoutBranchesAndTags();

        Tag[] GetAllTags();

        OriginBranch[] GetCurrentBranches();

        DateTime GetFirstNodeDate(OriginBranch branch);

        Node[] GetNodesConsecutive(OriginBranch branch);

        OriginBranch[] GetOrphanedBranches();

        void RemoveNodeFromBranch(OriginBranch branch, Node node);
        void RemoveNodeFromLeftOvers(Node n);

        void SetBranchNodes(OriginBranch branch, IEnumerable<Node> consecutiveNodes);

        // Leftovers.

        void SetLeftOverNodes(IEnumerable<Node> nodes);

        void SetTags(IEnumerable<Tag> allTags);

        // Branches.

        IEnumerable<Node> EnumerateNodesUpTheBranch(Node nodeInQuestion);
        IEnumerable<Node> EnumerateNodesDownTheBranch(Node nodeInQuestion);
        OriginBranch GetBranch(Node node);
        int GetIndexOnBranch(Node child);
    }
}