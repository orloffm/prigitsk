using System;
using System.Collections.Generic;
using Prigitsk.Core.Entities;
using Prigitsk.Framework;

namespace Prigitsk.Core.Graph
{
    public class Node : INode, IEquatable<Node>
    {
        private readonly List<ICommit> _absorbedParentCommitsList;
        private readonly int _hashCode;

        public Node(ICommit commit)
        {
            Commit = commit;
            _hashCode = commit.Hash.GetHashCode();

            ParentsSet = new OrderedSet<Node>();
            ChildrenSet = new HashSet<Node>();
            _absorbedParentCommitsList = new List<ICommit>();
        }

        public IEnumerable<ICommit> AbsorbedParentCommits => _absorbedParentCommitsList.WrapAsEnumerable();

        public IEnumerable<INode> Children => ChildrenSet.WrapAsEnumerable();

        public ICommit Commit { get; }

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
            return _hashCode;
        }

        public override string ToString()
        {
            return Commit.ToString();
        }

        internal void AddAbsorbedParent(Node node)
        {
            _absorbedParentCommitsList.AddRange(node._absorbedParentCommitsList);
            node._absorbedParentCommitsList.Clear();
            _absorbedParentCommitsList.Add(node.Commit);
        }
    }
}