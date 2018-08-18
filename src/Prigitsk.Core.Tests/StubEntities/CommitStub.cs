using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Tests.StubEntities
{
    public sealed class CommitStub : ICommit
    {
        public CommitStub()
        {
            Parents = new IHash[0];
        }

        public CommitStub(string hashValue, params ICommit[] parents)
            : this()
        {
            Hash = new HashStub(hashValue);
            Parents = parents.Select(c => c.Hash).ToArray();
        }

        public ISignature Author { get; set; }

        public ISignature Committer { get; set; }

        public IHash Hash { get; set; }

        public string Message { get; set; }

        public IEnumerable<IHash> Parents { get; set; }

        public string Treeish { get; set; }

        public bool Equals(ICommit other)
        {
            return ReferenceEquals(this, other);
        }
    }
}