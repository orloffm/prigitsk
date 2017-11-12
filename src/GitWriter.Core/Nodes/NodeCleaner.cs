using System.Collections.Generic;
using System.Linq;
using GitWriter.Core.Graph;

namespace GitWriter.Core.Nodes
{
    public class NodeCleaner : INodeCleaner
    {
        public void CleanUpGraph(
            IAssumedGraph graph,
            SimplificationOptions options)
        {
            // Remove nodes that are not used in the graph.
            // They are not needed.
            Node[] leftOutNodes = graph.EnumerateLeftOversWithoutBranchesAndTags().ToArray();
            for (int index = 0; index < leftOutNodes.Length; index++)
            {
                Node n = leftOutNodes[index];
                n.RemoveItselfFromTheNodeGraph();
                graph.RemoveNodeFromLeftOvers(n);
            }
            // If asked, don't simplify anything,
            if (options.PreventSimplification)
            {
                return;
            }
            int pass = 0;
            bool removedAnything;
            do
            {
                pass++;
                removedAnything = MakePass(graph, options);
            } while (removedAnything);
        }

        private bool CleanUpChildEdges(
            Node currentNode,
            Node nextNode,
            HashSet<Node> laterNodesInBranch,
            IAssumedGraph graph)
        {
            bool removedAnything = false;
            Node[] allChildren = currentNode.Children.ToArray();
            foreach (Node child in allChildren)
            {
                if (child == nextNode)
                {
                    // We always leave the main edge,
                    continue;
                }
                // Remove edges to other nodes below in the branch.
                // Remove edges to allChildren of the other nodes below,
                if (laterNodesInBranch.Contains(child) ||
                    laterNodesInBranch.Any(z => z.Children.Contains(child)))
                {
                    RemoveEdge(currentNode, child);
                    removedAnything = true;
                    continue;
                }
                // Remove edge to a child if there is an edge to a parent of the child:
                // the change was effectively merged already. This edge is a derivative of
                // some other simplification.
                bool parentIsLinked = IsSomeParentLinked(child, currentNode.Children, graph);
                if (parentIsLinked)
                {
                    // Remove this child.
                    RemoveEdge(currentNode, child);
                    removedAnything = true;
                    continue;
                }
                // Remove edge Al->B1 if it is a left side of a rhombus, see comments
                // for func.
                if (graph.GetIndexOnBranch(child) != 0)
                {
                    // The child is not the first node in the branch.
                    bool isRhombusStructure = IsLeftSideOfARhombus(currentNode, child, graph);
                    if (isRhombusStructure)
                    {
                        // Remove this child.
                        RemoveEdge(currentNode, child);
                        removedAnything = true;
                    }
                }
            }
            return removedAnything;
        }

        /// <summary>
        ///     Some child node An in the branch A contains an edge An->Bk
        ///     to the same branch B, and:
        ///     {no nodes	between	A1	and	An	have	other	parents},
        ///     {no nodes	between	B1	and	Bk	have	other	children}.
        ///     We call it a rhombus structure and Al->B1 the left side of that rhombus.
        ///     <para />
        ///     -- A1 — ... — Am — ... — An----------
        ///     <para />
        ///     \	\
        ///     <para />
        ///     -B1 — ... — Br —	... —	Bk —
        /// </summary>
        private bool IsLeftSideOfARhombus(
            Node currentNode,
            Node child,
            IAssumedGraph graph)
        {
            OriginBranch branchB = graph.GetBranch(child);
            if (branchB == null)
            {
                // Only branches, no free-floating nodes,
                return false;
            }
            // Enumerate later nodes in our branch.
            Node[] laterNodes = graph.EnumerateNodesDownTheBranch(currentNode).ToArray();
            foreach (Node an in laterNodes)
            {
                Node[] childrenOnBranchB = an.Children
                    .Where(c => graph.GetBranch(c) == branchB)
                    .ToArray();
                if (childrenOnBranchB.Length == 0)
                {
                    // This is Am, 1 < m < n.
                    // Does it have other parents?
                    if (an.Parents.Count > 1)
                    {
                        return false;
                    }
                    continue;
                }
                // Index of the nearest child Bn on B.
                int bnIndex = childrenOnBranchB.Select(graph.GetIndexOnBranch).Min();
                int blIndex = graph.GetIndexOnBranch(child);

                int countInBetween = bnIndex - blIndex - 1;

                Node[] childrenInBetween = graph.EnumerateNodesDownTheBranch(child).Take(countInBetween).ToArray();
                foreach (Node br in childrenInBetween)
                {
                    // This is Br, 1 < r < k.
                    // Does it have other children?
                    if (br.Children.Count > 1)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private bool IsDirectRoadToTip(
            Node node,
            OriginBranch branch)
        {
            Node n = node;
            while (true)
            {
                // Merging.
                if (n.Parents.Count != 1)
                {
                    return false;
                }
                // Is it a tip?
                if (n == branch.Source)
                {
                    return n.Children.Count == 0;
                }
                // Not yet a tip.
                if (n.Children.Count != 1)
                {
                    // Branching,
                    return false;
                }
                n = n.Children.Single();
            }
        }

        private bool IsSomeParentLinked(
            Node childNodeInQuestion,
            IEnumerable<Node> allChildren,
            IAssumedGraph graph)
        {
            // Is any of these children actually a parent of node in question?
            var otherChildren = new HashSet<Node>(allChildren);
            otherChildren.Remove(childNodeInQuestion);

            IEnumerable<Node> parents =
                graph.EnumerateNodesUpTheBranch(childNodeInQuestion);

            bool someParentIsLinked = parents.Any(
                parent =>
                    otherChildren.Contains(parent));

            return someParentIsLinked;
        }

        private bool MakePass(
            IAssumedGraph graph,
            SimplificationOptions options)
        {
            bool removedAnything = false;
            // Now remove extra edges.
            foreach (OriginBranch b in graph.GetCurrentBranches())
            {
                // All nodes in the branch.
                Node[] nodesInBranch = graph.GetNodesConsecutive(b);
                var laterNodesInBranchSet = new HashSet<Node>(nodesInBranch);
                for (int index = 0; index < nodesInBranch.Length; index++)
                {
                    Node currentNode = nodesInBranch[index];
                    laterNodesInBranchSet.Remove(currentNode);
                    Node nextNode = index < nodesInBranch.Length - 1 ? nodesInBranch[index + 1] : null;

                    // Remove edges that we got from cleaning up.
                    removedAnything |= CleanUpChildEdges(currentNode, nextNode, laterNodesInBranchSet, graph);
                    // We never remove the branch starting node.
                    if (options.AggressivelyRemoveFirstBranchNodes || index > 0)
                    {
                        // Now, can we remove this node altogether?
                        removedAnything |= RemoveNodeIfItIsOnlyConnecting(
                            graph,
                            currentNode,
                            b,
                            options.LeaveNodesAfterLastMerge);
                    }
                }
            }
            return removedAnything;
        }

        private void RemoveEdge(
            Node parent,
            Node child)
        {
            parent.Children.Remove(child);
            child.Parents.Remove(parent);
        }

        private bool RemoveNodeIfItIsOnlyConnecting(
            IAssumedGraph graph,
            Node currentNode,
            OriginBranch b,
            bool leaveNodesAfterLastMerge)
        {
            bool hasExplicitPointers = graph.AnyPointersAreSourcedFrom(currentNode);
            if (hasExplicitPointers)
            {
                return false;
            }

            bool canRemove = currentNode.Children.Count == 1 &&
                             currentNode.Parents.Count == 1;
            if (!canRemove)
            {
                return false;
            }

            if (leaveNodesAfterLastMerge)
            {
                // We leave nodes from whose there is a
                // road to tip - to investigate them in the graph.
                bool isDirectRoadToTip = IsDirectRoadToTip(currentNode, b);
                if (isDirectRoadToTip)
                {
                    return false;
                }
            }

            currentNode.RemoveItselfFromTheNodeGraph();
            graph.RemoveNodeFromBranch(b, currentNode);
            return true;
        }
    }
}