using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Framework;

namespace Prigitsk.Core.Graph
{
    public class Node : INode, IEquatable<Node>
    {
        private readonly List<ICommit> _absorbedParentCommitsList;
        private readonly int _hashCode;
        private readonly IHash _initialCommitHash;

        public Node(IHash initialCommitHash)
        {
            _initialCommitHash = initialCommitHash;
            _hashCode = initialCommitHash.GetHashCode();

            ParentsSet = new OrderedSet<Node>();
            ChildrenSet = new HashSet<Node>();
            _absorbedParentCommitsList = new List<ICommit>();
        }

        public IEnumerable<ICommit> AbsorbedParentCommits => _absorbedParentCommitsList.WrapAsEnumerable();

        public IEnumerable<INode> Children => ChildrenSet.WrapAsEnumerable();

        public ICommit Commit { get; private set; }

        public IEnumerable<INode> Parents => ParentsSet.WrapAsEnumerable();

        public string Treeish => _initialCommitHash.ToShortString();

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

        internal void SetCommit(ICommit commit)
        {
            Commit = commit;
        }
    }
}