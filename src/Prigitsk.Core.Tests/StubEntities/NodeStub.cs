using System;
using System.Collections.Generic;
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

        public NodeStub(ICommit commit)
            :this()
        {
            this.Commit = commit;
            this.Treeish = commit.Treeish;
        }

        public string Treeish { get; set; }

        public IEnumerable<ICommit> AbsorbedParentCommits { get; set; }

        public IEnumerable<INode> Children { get; set; }

        public ICommit Commit { get; set; }

        public IEnumerable<INode> Parents { get; set; }

        public bool Equals(INode other)
        {
            throw new NotImplementedException();
        }
    }
}