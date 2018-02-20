using System;
using System.Collections.Generic;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Tree
{
    public class Node : INode, IEquatable<Node>
    {
        private readonly int _initialHash;

        public Node(IHash initialCommitHash)
        {
            _initialHash = initialCommitHash.GetHashCode();

            Parents = new OrderedSet<INode>();
            Children = new HashSet<INode>();
            Commits = new OrderedSet<ICommit>();
        }

        public ISet<INode> Children { get; }

        public IOrderedSet<ICommit> Commits { get; }

        public IOrderedSet<INode> Parents { get; }

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
    }
}