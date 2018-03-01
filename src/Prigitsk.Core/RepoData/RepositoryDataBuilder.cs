using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.RepoData
{
    public sealed class RepositoryDataBuilder : IRepositoryDataBuilder
    {
        private readonly List<Branch> _branches;
        private readonly Dictionary<IHash, Commit> _commits;
        private readonly ILogger _logger;
        private readonly List<Remote> _remotes;
        private readonly List<Tag> _tags;

        public RepositoryDataBuilder(ILogger logger)
        {
            _logger = logger;

            _commits = new Dictionary<IHash, Commit>();
            _remotes = new List<Remote>();
            _branches = new List<Branch>();
            _tags = new List<Tag>();
        }

        public void AddCommit(string sha, string[] parentShas, DateTimeOffset committerWhen)
        {
            IHash hash = Hash.Create(sha);
            IHash[] parentHashes = parentShas.Select(Hash.Create).ToArray();

            Commit commit = new Commit(hash, parentHashes, committerWhen);
            _commits.Add(hash, commit);
        }

        public void AddRemote(string remoteName, string remoteUrl)
        {
            Remote r = new Remote(remoteName, remoteUrl);
            _remotes.Add(r);
        }

        public void AddRemoteBranch(string branchName, string tipSha)
        {
            IHash tip = Hash.Create(tipSha);
            Branch b = new Branch(branchName, tip);
            _branches.Add(b);
        }

        public void AddTag(string tagName, string tipSha)
        {
            IHash tip = Hash.Create(tipSha);
            Tag t = new Tag(tagName, tip);
            _tags.Add(t);
        }

        public IRepositoryData Build()
        {
            CheckData();

            return new RepositoryData(_commits.Values, _remotes, _branches, _tags);
        }

        private void CheckData()
        {
            // Each branch and tag must have a commit.
            foreach (Branch b in _branches)
            {
                if (!_commits.ContainsKey(b.Tip))
                {
                    throw new InvalidOperationException(
                        $"The commit {b.Tip} for branch {b} has not yet been loaded, cannot store data.");
                }
            }

            foreach (Tag t in _tags)
            {
                if (!_commits.ContainsKey(t.Tip))
                {
                    throw new InvalidOperationException(
                        $"The commit {t.Tip} for tag {t} has not yet been loaded, cannot store data.");
                }
            }
        }
    }
}