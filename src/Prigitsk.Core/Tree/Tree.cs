using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Tree
{
    public class Tree : ITree
    {
        private readonly IDictionary<IBranch, OrderedSet<INode>> _branches;
        private readonly IDictionary<INode, IBranch> _containedInBranch;
        private readonly IDictionary<IHash, INode> _nodes;
        private readonly IMultipleDictionary<INode, IBranch> _pointingBranches;
        private readonly IMultipleDictionary<INode, ITag> _pointingTags;
        private readonly ISet<ITag> _tags;

        public Tree()
        {
            _nodes = new Dictionary<IHash, INode>();
            _branches = new Dictionary<IBranch, OrderedSet<INode>>();
            _tags = new HashSet<ITag>();

            _containedInBranch = new Dictionary<INode, IBranch>();
            _pointingTags = new MultipleDictionary<INode, ITag>();
            _pointingBranches = new MultipleDictionary<INode, IBranch>();
        }

        public IEnumerable<IBranch> Branches => _branches.Keys.AsEnumerable();

        public IEnumerable<INode> Nodes => _nodes.Values.AsEnumerable();

        public void AddBranchWithCommits(IBranch branch, IEnumerable<ICommit> commitsInBranch)
        {
            var branchNodes = new OrderedSet<INode>();

            foreach (ICommit commit in commitsInBranch)
            {
                INode node = GetOrCreateNode(commit.Hash);
                branchNodes.Add(node);

                // Link it to the branch.
                _containedInBranch.Add(node, branch);
            }

            _branches.Add(branch, branchNodes);

            INode branchTip = GetOrCreateNode(branch.Tip);
            _pointingBranches.Add(branchTip, branch);
        }

        public void AddCommit(ICommit commit)
        {
            INode node = GetOrCreateNode(commit.Hash);
            node.Commit = commit;

            foreach (IHash parent in commit.Parents)
            {
                INode parentNode = GetOrCreateNode(parent);

                // Link both.
                node.Parents.Add(parentNode);
                parentNode.Children.Add(node);
            }
        }

        public void AddTag(ITag tag)
        {
            _tags.Add(tag);

            INode tagTip = GetOrCreateNode(tag.Tip);
            _pointingTags.Add(tagTip, tag);
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

        public IEnumerable<INode> EnumerateNodesDownTheBranch(INode node)
        {
            IBranch branch = _containedInBranch[node];
            OrderedSet<INode> set = _branches[branch];
            return set.EnumerateAfter(node);
        }

        public IEnumerable<INode> EnumerateNodesUpTheBranch(INode node)
        {
            IBranch branch = _containedInBranch[node];
            OrderedSet<INode> set = _branches[branch];
            return set.EnumerateBefore(node);
        }

        public INode FindOldestItemOnBranch(IEnumerable<INode> nodes)
        {
            INode[] nodesArray = nodes.ToArray();

            // Make sure these nodes are from the same branch.
            IBranch sourceBranch = nodesArray.Select(node => _containedInBranch[node]).Distinct().SingleOrDefault();
            if (sourceBranch == null)
            {
                return null;
            }

            OrderedSet<INode> set = _branches[sourceBranch];

            INode oldest = set.PickFirst(nodesArray);
            return oldest;
        }

        public IEnumerable<INode> GetAllBranchNodes(IBranch branch)
        {
            return _branches[branch];
        }

        public INode GetBranchTip(IBranch branch)
        {
            return _branches[branch].Last;
        }

        public IBranch GetContainingBranch(INode node)
        {
            IBranch branch;
            _containedInBranch.TryGetValue(node, out branch);
            return branch;
        }

        private INode GetOrCreateNode(IHash hash)
        {
            INode node;
            if (!_nodes.TryGetValue(hash, out node))
            {
                node = new Node(hash);
                _nodes.Add(hash, node);
            }

            return node;
        }

        public IEnumerable<IBranch> GetPointingBranches(INode node)
        {
            return _pointingBranches.TryEnumerateFor(node);
        }

        public IEnumerable<ITag> GetPointingTags(INode node)
        {
            return _pointingTags.TryEnumerateFor(node);
        }

        public bool IsStartingNodeOfBranch(INode node)
        {
            IBranch b = GetContainingBranch(node);
            return ReferenceEquals(_branches[b].First, node);
        }

        public void RemoveNode(INode n)
        {
            // We cannot remove nodes that have direct pointers on them.
            AssertNoBranchesOrTagsArePointing(n);

            // Parents.
            foreach (INode parent in n.Parents)
            {
                // Remove node.
                parent.Children.Remove(n);

                // Set its children to this parent.
                foreach (INode nChild in n.Children)
                {
                    parent.Children.Add(nChild);
                }
            }

            // Children.
            foreach (INode child in n.Children)
            {
                // Was this node the child's primary parent?
                bool isPrimary = n.Equals(child.Parents.First);
                // Anyway, remove the node from child's parents.
                child.Parents.Remove(n);

                if (isPrimary)
                {
                    // As the node was the primary parent, we add the parents in the beginning of the parents list.
                    foreach (INode nParent in n.Parents.Reverse())
                    {
                        child.Parents.AddFirst(nParent);
                    }
                }
                else
                {
                    // The node was not the primary parent. So we add its parents to the end of the list.
                    foreach (INode nParent in n.Parents)
                    {
                        child.Parents.AddLast(nParent);
                    }
                }
            }

            INode firstParent = n.Parents.FirstOrDefault();
            firstParent?.AddAbsorbedCommit(n.Commit);

            // Clear references.
            n.Parents.Clear();
            n.Children.Clear();

            // Remove it from the branch.
            _containedInBranch.Remove(n);
            // Remove it from the list of all nodes.
            _nodes.Remove(n.Commit.Hash);
        }
    }
}