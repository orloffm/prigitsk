using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Tree
{
    public class Node : INode, IEquatable<Node>
    {
        private readonly int _initialHash;
        private readonly List<ICommit> _absorbedCommitsList;

        public Node(IHash initialCommitHash)
        {
            _initialHash = initialCommitHash.GetHashCode();

            Parents = new OrderedSet<INode>();
            Children = new HashSet<INode>();
            _absorbedCommitsList = new List<ICommit>();
        }

        public IEnumerable<ICommit> AbsorbedCommits => _absorbedCommitsList.AsEnumerable();

        public ISet<INode> Children { get; }

        public ICommit Commit { get; set; }

        public IOrderedSet<INode> Parents { get; }

        public void AddAbsorbedCommit(ICommit commit)
        {
            _absorbedCommitsList.Add(commit);
        }

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