using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Graph
{
    public class Tree : ITree
    {
        private readonly IDictionary<IBranch, OrderedSet<Node>> _branches;
        private readonly IDictionary<Node, IBranch> _containedInBranch;
        private readonly IDictionary<IHash, Node> _nodes;
        private readonly IMultipleDictionary<Node, IBranch> _pointingBranches;
        private readonly IMultipleDictionary<Node, ITag> _pointingTags;
        private readonly ISet<ITag> _tags;

        public Tree()
        {
            _nodes = new Dictionary<IHash, Node>();
            _branches = new Dictionary<IBranch, OrderedSet<Node>>();
            _tags = new HashSet<ITag>();

            _containedInBranch = new Dictionary<Node, IBranch>();
            _pointingTags = new MultipleDictionary<Node, ITag>();
            _pointingBranches = new MultipleDictionary<Node, IBranch>();
        }

        public IEnumerable<IBranch> Branches => _branches.Keys.WrapAsEnumerable();

        public IEnumerable<INode> Nodes => _nodes.Values.WrapAsEnumerable();

        public IEnumerable<ITag> Tags => _tags.WrapAsEnumerable();

        public void AddBranch(IBranch branch, IEnumerable<IHash> hashesInBranch)
        {
            var branchNodes = new OrderedSet<Node>();

            foreach (IHash hash in hashesInBranch)
            {
                Node node = GetOrCreateNode(hash);
                branchNodes.Add(node);

                // Link it to the branch.
                _containedInBranch.Add(node, branch);
            }

            // Branch and all its nodes ordered from the start. The tip is the last one.
            _branches.Add(branch, branchNodes);

            // Especially save the tip.
            Node branchTip = GetOrCreateNode(branch.Tip);
            _pointingBranches.Add(branchTip, branch);
        }

        public void AddCommit(ICommit commit)
        {
            Node node = GetOrCreateNode(commit.Hash);
            node.SetCommit(commit);

            foreach (IHash parent in commit.Parents)
            {
                Node parentNode = GetOrCreateNode(parent);

                // Link both.
                node.ParentsSet.Add(parentNode);
                parentNode.ChildrenSet.Add(node);
            }
        }

        public void AddTag(ITag tag)
        {
            _tags.Add(tag);

            Node tagTip = GetOrCreateNode(tag.Tip);
            _pointingTags.Add(tagTip, tag);
        }

        public void DropTag(ITag tag)
        {
            IHash pointsTo = tag.Tip;
            Node node = Unwrap(pointsTo);

            _pointingTags.Remove(node, tag);
            _tags.Remove(tag);
        }

        public IEnumerable<INode> EnumerateNodes(IBranch branch)
        {
            return _branches[branch].WrapAsEnumerable();
        }

        public IEnumerable<INode> EnumerateNodesDownTheBranch(INode inode)
        {
            Node node = Unwrap(inode);
            IBranch branch = _containedInBranch[node];
            OrderedSet<Node> set = _branches[branch];
            return set.EnumerateAfter(node);
        }

        public IEnumerable<INode> EnumerateNodesUpTheBranch(INode inode)
        {
            Node node = Unwrap(inode);
            IBranch branch = _containedInBranch[node];
            OrderedSet<Node> set = _branches[branch];
            return set.EnumerateBefore(node);
        }

        public INode FindOldestItemOnBranch(IEnumerable<INode> nodes)
        {
            Node[] nodesArray = nodes.Select(Unwrap).ToArray();

            // Make sure these nodes are from the same branch.
            IBranch sourceBranch = nodesArray.Select(node => _containedInBranch[node]).Distinct().SingleOrDefault();
            if (sourceBranch == null)
            {
                return null;
            }

            OrderedSet<Node> set = _branches[sourceBranch];

            INode oldest = set.PickFirst(nodesArray);
            return oldest;
        }

        public INode GetBranchTip(IBranch branch)
        {
            return _branches[branch].Last;
        }

        public IBranch GetContainingBranch(INode inode)
        {
            Node node = Unwrap(inode);
            IBranch branch;
            _containedInBranch.TryGetValue(node, out branch);
            return branch;
        }

        public INode GetNode(IHash ihash)
        {
            _nodes.TryGetValue(ihash, out Node value);
            return value;
        }

        public IEnumerable<IBranch> GetPointingBranches(INode node)
        {
            Node n = Unwrap(node);
            return _pointingBranches.TryEnumerateFor(n);
        }

        public IEnumerable<ITag> GetPointingTags(INode node)
        {
            Node n = Unwrap(node);
            return _pointingTags.TryEnumerateFor(n);
        }

        public bool IsStartingNodeOfBranch(INode inode)
        {
            Node node = Unwrap(inode);
            IBranch b = GetContainingBranch(node);

            if (b == null)
            {
                // No branch at all.
                return false;
            }

            return _branches[b].First == node;
        }

        public void RemoveEdge(INode parent, INode child)
        {
            Node parentNode = Unwrap(parent);
            Node childNode = Unwrap(child);

            parentNode.ChildrenSet.Remove(childNode);
            childNode.ParentsSet.Remove(parentNode);
        }

        public void RemoveNode(INode inode)
        {
            Node n = Unwrap(inode);

            // We cannot remove nodes that have direct pointers on them.
            AssertNoBranchesOrTagsArePointing(n);

            // Parents.
            foreach (Node parent in n.ParentsSet)
            {
                // Remove node from parents' children.
                parent.ChildrenSet.Remove(n);

                // Set its children to this parent.
                foreach (Node nChild in n.ChildrenSet)
                {
                    parent.ChildrenSet.Add(nChild);
                }
            }

            // Children.
            foreach (Node child in n.ChildrenSet)
            {
                // Was this node the child's primary parent?
                bool isPrimary = n == child.ParentsSet.First;
                // Anyway, remove the node from child's parents.
                child.ParentsSet.Remove(n);

                if (isPrimary)
                {
                    // As the node was the primary parent of this child,
                    // we add its parents in the beginning of the child's parents list.
                    foreach (Node nParent in n.ParentsSet.Reverse())
                    {
                        child.ParentsSet.AddFirst(nParent);
                    }
                }
                else
                {
                    // The node was not the primary parent of this child,
                    // so we add its parents to the end of the list.
                    foreach (Node nParent in n.ParentsSet)
                    {
                        child.ParentsSet.AddLast(nParent);
                    }
                }

                // Add this node to the list of absorbed ones.
                child.AddAbsorbedParent(n);
            }

            // Clear references.
            n.ParentsSet.Clear();
            n.ChildrenSet.Clear();

            IBranch b;
            _containedInBranch.TryGetValue(n, out b);
            if (b != null)
            {
                // Remove its pointer to its branch.
                _containedInBranch.Remove(n);

                // Remove from the nodes of the branch.
                _branches[b].Remove(n);
            }

            // Remove node from the list of all nodes.
            _nodes.Remove(n.Commit.Hash);
        }

        private void AssertNoBranchesOrTagsArePointing(INode node)
        {
            IBranch b = GetPointingBranches(node).FirstOrDefault();
            if (b != null)
            {
                throw new InvalidOperationException($"Cannot remove node {node} as branch {b} points directly to it.");
            }

            ITag t = GetPointingTags(node).FirstOrDefault();
            if (t != null)
            {
                throw new InvalidOperationException($"Cannot remove node {node} as tag {t} points directly to it.");
            }
        }

        private Node GetOrCreateNode(IHash hash)
        {
            Node node;
            if (!_nodes.TryGetValue(hash, out node))
            {
                node = new Node(hash);
                _nodes.Add(hash, node);
            }

            return node;
        }

        private Node Unwrap(IHash ihash)
        {
            return _nodes[ihash];
        }

        private Node Unwrap(INode inode)
        {
            return Unwrap(inode.Commit.Hash);
        }
    }
}