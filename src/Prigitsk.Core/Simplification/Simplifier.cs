using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Tree;

namespace Prigitsk.Core.Simplification
{
    public sealed class Simplifier : ISimplifier
    {
        private readonly ILogger _log;
        private readonly ITreeWalker _walker;

        public Simplifier(ILogger log, ITreeWalker walker)
        {
            _log = log;
            _walker = walker;
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

        private bool CleanUpChildEdges(
            INode currentNode,
            INode nextNode,
            ICollection<INode> laterNodesInBranch,
            ITree tree)
        {
            bool removedAnything = false;
            INode[] allChildren = currentNode.Children.ToArray();
            foreach (INode child in allChildren)
            {
                if (ReferenceEquals(child, nextNode))
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
                bool parentIsLinked = IsSomeParentLinked(currentNode, child, currentNode.Children);
                if (parentIsLinked)
                {
                    // Remove this child.
                    RemoveEdge(currentNode, child);
                    removedAnything = true;
                    continue;
                }

                // Remove edge Al->B1 if it is a left side of a rhombus, see comments
                // for func.
                if (!tree.IsStartingNodeOfBranch(child))
                {
                    // The child is not the first node in the branch.
                    bool isRhombusStructure = IsLeftSideOfARhombus(currentNode, child, tree);
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

        private IEnumerable<INode> EnumerateNodesBetween(ITree tree, INode a, INode b)
        {
            return tree.EnumerateNodesDownTheBranch(a).TakeWhile(node => node != b);
        }

        /// <summary>
        ///     Is the branch tip accessible the direct road from this node.
        /// </summary>
        private bool IsDirectRoadToTip(
            ITree tree,
            INode node)
        {
            INode n = node;
            IBranch b = tree.GetContainingBranch(node);
            INode tip = tree.GetBranchTip(b);

            while (true)
            {
                // Merging.
                if (n.Parents.Count != 1)
                {
                    return false;
                }

                // Is it a tip?
                if (n == tip)
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
            ITree tree)
        {
            IBranch branchB = tree.GetContainingBranch(child);

            INode an, bk;
            bool foundClosure = TryFindRightSideOfARhombus(tree, parent, branchB, out an, out bk);
            if (!foundClosure)
            {
                // If there is no right side to the rhombus, don't care.
                return false;
            }

            IEnumerable<INode> parentsInBetween = EnumerateNodesBetween(tree, parent, an);
            INode[] childrenInBetween = EnumerateNodesBetween(tree, child, bk).ToArray();

            // Children may have third level of nodes on: themselves, their parents and the last elements.
            INode[] possibleThirdGeneration =
                childrenInBetween.Concat(parentsInBetween).Concat(new[] {an, bk}).ToArray();

            // Check that these children don't have other children.
            // In other words - that the left side doesn't leak.
            bool noOtherChildren = CheckAllNonPrimaryChildrenAreFrom(childrenInBetween, possibleThirdGeneration);
            return noOtherChildren;
        }

        private bool IsSomeParentLinked(
            INode sourceNode,
            INode childNodeInQuestion,
            IEnumerable<INode> allChildren)
        {
            // Is any of these children actually a parent of node in question?
            var otherChildren = new HashSet<INode>(allChildren);
            otherChildren.Remove(childNodeInQuestion);

            // The whole parent tree of the particular child node in question. We don't go earlier than the source node.
            IEnumerable<INode> parents = _walker.EnumerateAllParentsBreadthFirst(
                childNodeInQuestion,
                sourceNode.Commit.CommittedWhen);

            // Is any of them linked from other children of source node?
            bool someParentIsLinked = parents.Any(otherChildren.Contains);

            return someParentIsLinked;
        }

        private bool MakePass(ITree tree, ISimplificationOptions options)
        {
            bool removedAnything = false;
            IBranch[] branches = tree.Branches.ToArray();

            foreach (IBranch b in branches)
            {
                INode[] nodesInBranch = tree.GetAllBranchNodes(b).ToArray();
                var laterNodesInBranchSet = new HashSet<INode>(nodesInBranch);

                for (int index = 0; index < nodesInBranch.Length; index++)
                {
                    INode currentNode = nodesInBranch[index];
                    laterNodesInBranchSet.Remove(currentNode);
                    INode nextNode = index < nodesInBranch.Length - 1 ? nodesInBranch[index + 1] : null;

                    // Remove edges that we got from cleaning up.
                    removedAnything |= CleanUpChildEdges(currentNode, nextNode, laterNodesInBranchSet, tree);
                    // We never remove the branch starting node.
                    if (options.AggressivelyRemoveFirstBranchNodes || index > 0)
                    {
                        // Now, can we remove this node altogether?
                        removedAnything |= RemoveNodeIfItIsOnlyConnecting(
                            tree,
                            currentNode,
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

        private bool RemoveNodeIfItIsOnlyConnecting(ITree tree, INode currentNode, bool optionsLeaveNodesAfterLastMerge)
        {
            bool hasExplicitPointers =
                tree.GetPointingBranches(currentNode).Any() || tree.GetPointingTags(currentNode).Any();
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

            if (optionsLeaveNodesAfterLastMerge)
            {
                // We leave nodes from whose there is a
                // road to tip - to investigate them in the graph.
                bool isDirectRoadToTip = IsDirectRoadToTip(tree, currentNode);
                if (isDirectRoadToTip)
                {
                    return false;
                }
            }

            tree.RemoveNode(currentNode);
            return true;
        }

        /// <summary>
        ///     Removes orphaned nodes (i.e. not contained in branches) that don't have tags.
        /// </summary>
        private void RemoveOrphans(ITree tree, bool removeOrphansEvenWithTags)
        {
            INode[] nodes = tree.Nodes.ToArray();

            foreach (INode node in nodes)
            {
                // It shouldn't be on any branch.
                IBranch containingBranch = tree.GetContainingBranch(node);
                if (containingBranch != null)
                {
                    continue;
                }

                if (!removeOrphansEvenWithTags)
                {
                    // It shouldn't have tags.
                    ITag someTag = tree.GetPointingTags(node).FirstOrDefault();
                    if (someTag != null)
                    {
                        continue;
                    }
                }

                tree.RemoveNode(node);
            }
        }

        public void Simplify(ITree tree, ISimplificationOptions options)
        {
            // First, remove orphans, if applicable.
            if (options.RemoveOrphans)
            {
                RemoveOrphans(tree, options.RemoveOrphansEvenWithTags);
            }

            int pass = 0;
            bool removedAnything;
            do
            {
                _log.Debug("Doing pass {0}.", pass + 1);
                pass++;
                removedAnything = MakePass(tree, options);
            } while (removedAnything);
        }

        private bool TryFindRightSideOfARhombus(
            ITree tree,
            INode parent,
            IBranch branchB,
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

            foreach (INode am in tree.EnumerateNodesDownTheBranch(parent))
            {
                IEnumerable<INode> allAmChildrenOnB = am.Children.Where(c => tree.GetContainingBranch(c) == branchB);
                INode leftmostChildOnB = tree.FindOldestItemOnBranch(allAmChildrenOnB);

                if (leftmostChildOnB != null)
                {
                    an = am;
                    bk = leftmostChildOnB;
                    return true;
                }
            }

            return false;
        }
    }
}