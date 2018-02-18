﻿using System;
using System.Collections.Generic;

namespace Prigitsk.Core.Entities
{
    public sealed class Commit : IEquatable<Commit>, ICommit
    {
        public Commit(IHash hash, IHash[] parentsHashes, DateTimeOffset? commiterTimeStamp)
        {
            Parents = new List<IHash>(parentsHashes);
            Hash = hash;
            CommittedWhen = commiterTimeStamp;
        }

        public IHash Hash { get; }
        public IEnumerable<IHash> Parents { get; }
        public DateTimeOffset? CommittedWhen { get; }

        public bool Equals(ICommit other)
        {
            return AreEqual(this, other);
        }

        public bool Equals(Commit other)
        {
            return AreEqual(this, other);
        }

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

        public override bool Equals(object obj)
        {
            return AreEqual(this, obj as ICommit);
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }
    }
}