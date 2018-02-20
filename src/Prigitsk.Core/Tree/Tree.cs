using System.Collections.Generic;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Tree
{
    public class Tree : ITree
    {
        private readonly IDictionary<IBranch, OrderedSet<Node>> _branches;
        private readonly IDictionary<INode, IBranch> _containedInBranch;
        private readonly IMultipleDictionary<INode, ITag> _pointingTags;
        private readonly IMultipleDictionary<INode, IBranch> _pointingBranches;
        private readonly IDictionary<IHash, Node> _nodes;
        private readonly ISet<ITag> _tags;

        public Tree()
        {
            _nodes = new Dictionary<IHash, Node>();
            _branches = new Dictionary<IBranch, OrderedSet<Node>>();
            _tags = new HashSet<ITag>();

            _containedInBranch = new Dictionary<INode, IBranch>();
            _pointingTags = new MultipleDictionary<INode, ITag>();
            _pointingBranches = new MultipleDictionary<INode, IBranch>();
        }

        public void AddBranchCommits(IBranch branch, IEnumerable<ICommit> commitsInBranch)
        {
            var branchNodes = new OrderedSet<Node>();

            foreach (ICommit commit in commitsInBranch)
            {
                Node node = GetOrCreateNodeFromCommit(commit);
                branchNodes.Add(node);
            }

            _branches.Add(branch, branchNodes);
        }

        public void AddCommitsWithoutBranches(IEnumerable<ICommit> commits)
        {
            foreach (ICommit commit in commits)
            {
                GetOrCreateNodeFromCommit(commit);
            }
        }

        public void AddTags(IEnumerable<ITag> tags)
        {
            foreach (ITag tag in tags)
            {
                _tags.Add(tag);
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

        private Node GetOrCreateNodeFromCommit(ICommit commit)
        {
            Node node = GetOrCreateNode(commit.Hash);
            node.Commits.Add(commit);

            foreach (IHash parent in commit.Parents)
            {
                Node parentNode = GetOrCreateNode(parent);

                // Link both.
                node.Parents.Add(parentNode);
                parentNode.Children.Add(node);
            }

            return node;
        }
    }
}