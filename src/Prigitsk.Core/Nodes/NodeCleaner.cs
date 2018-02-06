using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Graph;

namespace Prigitsk.Core.Nodes
{
    public class NodeCleaner : INodeCleaner
    {
        private readonly ITreeManipulator _manipulator;
        private readonly ITreeWalker _walker;

        public NodeCleaner(ITreeManipulator manipulator, ITreeWalker walker)
        {
            _manipulator = manipulator;
            _walker = walker;
        }

        public void CleanUpGraph(
            IAssumedGraph graph,
            SimplificationOptions options)
        {
            // Remove nodes that are not used in the graph.
            // They are not needed.
            INode[] leftOutNodes = graph.EnumerateLeftOversWithoutBranchesAndTags().ToArray();
            for (int index = 0; index < leftOutNodes.Length; index++)
            {
                INode n = leftOutNodes[index];
                _manipulator.RemoveItselfFromTheNodeGraph(n);
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
            INode currentNode,
            INode nextNode,
            ICollection<INode> laterNodesInBranch,
            IAssumedGraph graph)
        {
            bool removedAnything = false;
            INode[] allChildren = currentNode.Children.ToArray();
            foreach (INode child in allChildren)
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
                bool parentIsLinked = IsSomeParentLinked(currentNode, child, currentNode.Children, graph);
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
        ///     {no nodes between A0 and An have other parents},
        ///     {no nodes between B0 and Bk have other children}.
        ///     We call it a rhombus structure and Al->B1 the left side of that rhombus.
        ///     <para />
        ///     -- A0 -- ... --- Am -- ... -- An----------
        ///     <para />
        ///     \            /             \
        ///     <para />
        ///     --- B0 -- ... - Br -- ... ---- Bk --
        /// </summary>
        private bool IsLeftSideOfARhombus(
            INode parent,
            INode child,
            IAssumedGraph graph)
        {
            OriginBranch branchB = graph.GetBranch(child);

            INode an, bk;
            bool foundClosure = TryFindRightSideOfARhombus(graph, parent, branchB, out an, out bk);
            if (!foundClosure)
            {
                // If there is no right side to the rhombus, don't care.
                return false;
            }

            IEnumerable<INode> parentsInBetween = EnumerateNodesBetween(graph, parent, an);
            INode[] childrenInBetween = EnumerateNodesBetween(graph, child, bk).ToArray();

            // Children may have third level of nodes on: themselves, their parents and the last elements.
            INode[] possibleThirdGeneration =
                childrenInBetween.Concat(parentsInBetween).Concat(new[] {an, bk}).ToArray();

            // Check that these children don't have other children.
            // In other words - that the left side doesn't leak.
            bool noOtherChildren = CheckAllNonPrimaryChildrenAreFrom(childrenInBetween, possibleThirdGeneration);
            return noOtherChildren;
        }

        private bool CheckAllNonPrimaryChildrenAreFrom(IEnumerable<INode> nodesInQuestion, IEnumerable<INode> whitelist)
        {
            var whitelistSet = new HashSet<INode>(whitelist);

            foreach (INode node in nodesInQuestion)
            {
                foreach (INode c in node.Children)
                {
                    bool childIsInWhitelist = whitelistSet.Contains(c);
                    if (!childIsInWhitelist)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private IEnumerable<INode> EnumerateNodesBetween(IAssumedGraph graph, INode parent, INode an)
        {
            foreach (INode node in graph.EnumerateNodesDownTheBranch(parent))
            {
                if (node == an)
                {
                    yield break;
                }

                yield return node;
            }
        }

        private bool TryFindRightSideOfARhombus(
            IAssumedGraph graph,
            INode parent,
            OriginBranch branchB,
            out INode an,
            out INode bk)
        {
            an = null;
            bk = null;

            if (branchB == null)
            {
                // Only branches, no free-floating nodes,
                return false;
            }

            foreach (INode am in graph.EnumerateNodesDownTheBranch(parent))
            {
                INode leftmostChildOnB = am.Children.Where(c => graph.GetBranch(c) == branchB)
                    .OrderBy(graph.GetIndexOnBranch)
                    .FirstOrDefault();

                if (leftmostChildOnB != null)
                {
                    an = am;
                    bk = leftmostChildOnB;
                    return true;
                }
            }

            return false;
        }

        private bool IsDirectRoadToTip(
            INode node,
            OriginBranch branch)
        {
            INode n = node;
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
            INode sourceNode,
            INode childNodeInQuestion,
            IEnumerable<INode> allChildren,
            IAssumedGraph graph)
        {
            // Is any of these children actually a parent of node in question?
            var otherChildren = new HashSet<INode>(allChildren);
            otherChildren.Remove(childNodeInQuestion);

            // The whole parent tree of the particular child node in question. We don't go earlier than the source node.
            IEnumerable<INode> parents = _walker.EnumerateAllParentsBreadthFirst(childNodeInQuestion, sourceNode.Time);

            // Is any of them linked from other children of source node?
            bool someParentIsLinked = parents.Any(otherChildren.Contains);

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
                INode[] nodesInBranch = graph.GetNodesConsecutive(b);
                var laterNodesInBranchSet = new HashSet<INode>(nodesInBranch);
                for (int index = 0; index < nodesInBranch.Length; index++)
                {
                    INode currentNode = nodesInBranch[index];
                    laterNodesInBranchSet.Remove(currentNode);
                    INode nextNode = index < nodesInBranch.Length - 1 ? nodesInBranch[index + 1] : null;

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
            INode parent,
            INode child)
        {
            parent.Children.Remove(child);
            child.Parents.Remove(parent);
        }

        private bool RemoveNodeIfItIsOnlyConnecting(
            IAssumedGraph graph,
            INode currentNode,
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

            _manipulator.RemoveItselfFromTheNodeGraph(currentNode);
            graph.RemoveNodeFromBranch(b, currentNode);
            return true;
        }
    }
}