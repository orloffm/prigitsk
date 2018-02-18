using System;
using System.Collections.Generic;

namespace Prigitsk.Core.RepoData
{
    public sealed class Commit : IEquatable<Commit>
    {
        public Commit(string sha)
        {
            Parents = new List<Commit>();
            Sha = sha;
        }

        public string Sha { get; }
        public List<Commit> Parents { get; }
        public DateTimeOffset? CommittedWhen { get; set; }

        public bool Equals(Commit other)
        {
            return AreEqual(this, other);
        }

        public static bool AreEqual(Commit a, Commit b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(null, b))
            {
                return false;
            }

            return string.Equals(a.Sha, b.Sha, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return AreEqual(this, obj as Commit);
        }

        public override int GetHashCode()
        {
            return Sha.GetHashCode();
        }
    }
}