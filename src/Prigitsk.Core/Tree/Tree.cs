using System.Collections.Generic;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Tree
{
    public class Tree : ITree
    {
        private readonly Dictionary<IBranch, OrderedSet<Node>> _branches;
        private readonly HashSet<ITag> _tags;
        private readonly Dictionary<IHash, Node> _nodes;

        public Tree()
        {
            _nodes = new Dictionary<IHash, Node>();
            _branches = new Dictionary<IBranch, OrderedSet<Node>>();
            _tags = new HashSet<ITag>();
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

        public void AddTags(IEnumerable<ITag> tags)
        {
            foreach (var tag in tags)
                _tags.Add(tag);
        }

        public void AddCommitsWithoutBranches(IEnumerable<ICommit> commits)
        {
            foreach (ICommit commit in commits)
                 GetOrCreateNodeFromCommit(commit);
        }

        private Node GetOrCreateNodeFromCommit(ICommit commit)
        {
            Node node= GetOrCreateNode(commit.Hash);
            node.Commits.Add(commit);

            foreach (IHash parent in commit.Parents)
            {
                Node parentNode=   GetOrCreateNode(parent);

                // Link both.
                node.Parents.Add(parentNode);
                parentNode.Children.Add(node);
            }

            return node;
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