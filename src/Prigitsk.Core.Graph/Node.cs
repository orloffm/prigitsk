using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Framework;

namespace Prigitsk.Core.Graph
{
    public class Node : INode, IEquatable<Node>
    {
        private readonly List<Node> _absorbedParentsList;
        private readonly int _hashCode;
        private readonly IHash _initialCommitHash;

        public Node(IHash initialCommitHash)
        {
            _initialCommitHash = initialCommitHash;
            _hashCode = initialCommitHash.GetHashCode();

            ParentsSet = new OrderedSet<Node>();
            ChildrenSet = new HashSet<Node>();
            _absorbedParentsList = new List<Node>();
        }

        public IEnumerable<ICommit> AbsorbedParentCommits => _absorbedParentsList.Select(n => n.Commit);

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
            _absorbedParentsList.AddRange(node._absorbedParentsList);
            _absorbedParentsList.Add(node);
        }

        internal void SetCommit(ICommit commit)
        {
            Commit = commit;
        }
    }
}