﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.RepoData
{
    public sealed class CommitsData : ICommitsData
    {
        private readonly Dictionary<IHash, ICommit> _commits;

        public CommitsData(IEnumerable<ICommit> data)
        {
            _commits = data.ToDictionary(item => item.Hash, item => item);
        }

        public int Count => _commits.Count;

        public IEnumerable<ICommit> EnumerateUpTheHistoryFrom(ICommit tip)
        {
            ICommit n = tip;
            do
            {
                yield return n;
                n = GetByHash(n.Parents.FirstOrDefault());
            } while (n != null);
        }

        public ICommit GetByHash(IHash hash)
        {
            if (hash == null)
            {
                return null;
            }

            ICommit commit;
            _commits.TryGetValue(hash, out commit);
            return commit;
        }

        public IEnumerator<ICommit> GetEnumerator()
        {
            return _commits.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}