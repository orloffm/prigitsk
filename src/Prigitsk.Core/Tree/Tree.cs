using System.Collections.Generic;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Tree
{
    public class Tree : ITree
    {
        private readonly IDictionary<IBranch, OrderedSet<Node>> _branches;
        private readonly IDictionary<INode, IBranch> _containedInBranch;
        private readonly IDictionary<IHash, Node> _nodes;
        private readonly IMultipleDictionary<INode, IBranch> _pointingBranches;
        private readonly IMultipleDictionary<INode, ITag> _pointingTags;
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

        public void AddBranchWithCommits(IBranch branch, IEnumerable<ICommit> commitsInBranch)
        {
            var branchNodes = new OrderedSet<Node>();

            foreach (ICommit commit in commitsInBranch)
            {
                Node node = GetOrCreateNode(commit.Hash);
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
            Node node = GetOrCreateNode(commit.Hash);
            node.Commit = commit;

            foreach (IHash parent in commit.Parents)
            {
                Node parentNode = GetOrCreateNode(parent);

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
    }
}