using System;
using System.Collections.Generic;

namespace Prigitsk.Core.Entities
{
    public sealed class Commit : IEquatable<Commit>, ICommit
    {
        public Commit(
            IHash hash,
            IEnumerable<IHash> parentsHashes,
            ISignature author,
            ISignature committer,
            string message)
        {
            Parents = new List<IHash>(parentsHashes);
            Hash = hash;
            Author = author;
            Committer = committer;
            Message = message;
        }

        public ISignature Author { get; }

        public ISignature Committer { get; }

        public IHash Hash { get; }

        public string Message { get; }

        public IEnumerable<IHash> Parents { get; }

        public string Treeish => Hash.ToShortString();

        public static bool AreEqual(ICommit a, ICommit b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(null, b))
            {
                return false;
            }

            return a.Hash.Equals(b.Hash);
        }

        public bool Equals(ICommit other)
        {
            return AreEqual(this, other);
        }

        public bool Equals(Commit other)
        {
            return AreEqual(this, other);
        }

        public override bool Equals(object obj)
        {
            return AreEqual(this, obj as ICommit);
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }

        public override string ToString()
        {
            return Hash.ToString();
        }
    }
}