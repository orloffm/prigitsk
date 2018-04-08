using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Graph
{
    public class Node : INode, IEquatable<Node>
    {
        private readonly List<Node> _absorbedNodesList;
        private readonly int _initialHash;

        public Node(IHash initialCommitHash)
        {
            _initialHash = initialCommitHash.GetHashCode();

            ParentsSet = new OrderedSet<Node>();
            ChildrenSet = new HashSet<Node>();
            _absorbedNodesList = new List<Node>();
        }

        public IEnumerable<ICommit> AbsorbedChildCommits => _absorbedNodesList.Select(n => n.Commit);

        public IEnumerable<INode> Children => ChildrenSet.WrapAsEnumerable();

        public ICommit Commit { get; private set; }

        public IEnumerable<INode> Parents => ParentsSet.WrapAsEnumerable();

        internal ISet<Node> ChildrenSet { get; }

        internal IOrderedSet<Node> ParentsSet { get; }

        public static bool AreEqual(INode a, INode b)
        {
            return ReferenceEquals(a, b);
        }

        public bool Equals(INode other)
        {
            return AreEqual(this, other);
        }

        public bool Equals(Node other)
        {
            return AreEqual(this, other);
        }

        public override bool Equals(object obj)
        {
            return AreEqual(this, obj as INode);
        }

        public override int GetHashCode()
        {
            return _initialHash;
        }

        public override string ToString()
        {
            return Commit.ToString();
        }

        internal void AddAbsorbedChild(Node node)
        {
            _absorbedNodesList.Add(node);
            _absorbedNodesList.AddRange(node._absorbedNodesList);
        }

        internal void SetCommit(ICommit commit)
        {
            Commit = commit;
        }
    }
}