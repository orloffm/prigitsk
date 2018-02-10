using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Prigitsk.Core.RepoData;

namespace Prigitsk.Core.Nodes.Loading
{
    public sealed class RepositoryDataBuilder : IRepositoryDataBuilder
    {
        private readonly ILogger _logger;
        private readonly Dictionary<string, Commit> _commits;
        private readonly Dictionary<string, string> _remotes;
        private readonly Dictionary<string, Commit> _branches;
        private readonly Dictionary<string, Commit> _tags;

        public RepositoryDataBuilder(ILogger logger)
        {
            _logger = logger;

            _commits = new Dictionary<string, Commit>();
            _remotes = new Dictionary<string, string>();
            _branches = new Dictionary<string, Commit>();
            _tags = new Dictionary<string, Commit>();
        }

        public IRepositoryData Build()
        {
            return new RepositoryData(_commits,_remotes,_branches,_tags);
        }

        public void AddCommit(string sha, string[] parentShas, DateTimeOffset committerWhen)
        {
            var commit = GetOrCreateCommit(sha);
            commit.CommittedWhen = committerWhen;
            foreach (var parentCommit in parentShas.Select(GetOrCreateCommit))
            {
                commit.Parents.Add(parentCommit);
            }
        }

        private Commit GetOrCreateCommit(string sha)
        {
          string cut = this.CutSha(sha);
            Commit ret;
            if(!_commits.TryGetValue(cut, out ret))
            {
                ret = new Commit(cut);
                _commits.Add(cut,ret);
            }
            return ret;
        }

        public void AddRemote(string remoteName, string rUrl)
        {
            _remotes.Add(remoteName, rUrl);
        }

        public void AddBranch(string branchName, string tipSha)
        {
          _branches.Add(branchName, GetOrCreateCommit(tipSha));
        }

        public void AddTag(string tagName, string tipSha)
        {
          _branches.Add(tagName, GetOrCreateCommit(tipSha));
        }

        private string CutSha(string sha)
        {
            return sha.Substring(0, 7);
        }
    }
}