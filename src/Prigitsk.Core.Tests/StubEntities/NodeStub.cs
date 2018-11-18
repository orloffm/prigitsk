using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Graph;

namespace Prigitsk.Core.Tests.StubEntities
{
    public sealed class NodeStub : INode
    {
        public NodeStub()
        {
            AbsorbedParentCommits = new ICommit[0];
            Children = new INode[0];
            Parents = new INode[0];
        }

        public NodeStub(ICommit commit) : this()
        {
            Commit = commit;
        }

        public NodeStub(ICommit commit, INode[] parents) : this(commit)
        {
            Parents = parents.ToArray();
        }

        public IEnumerable<ICommit> AbsorbedParentCommits { get; set; }

        public IEnumerable<INode> Children { get; set; }

        public ICommit Commit { get; }

        public IEnumerable<INode> Parents { get; set; }

        public bool Equals(INode other)
        {
            return ReferenceEquals(this, other);
        }

        public override string ToString()
        {
            return Commit?.Treeish;
        }
    }
}