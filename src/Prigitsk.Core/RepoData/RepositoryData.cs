using System.Collections.Generic;
using Prigitsk.Core.Nodes.Loading;

namespace Prigitsk.Core.RepoData
{
    public class RepositoryData : IRepositoryData
    {
        private Dictionary<string, Commit> _branches;
        private Dictionary<string, Commit> _commits;
        private Dictionary<string, string> _remotes;
        private Dictionary<string, Commit> _tags;

        public RepositoryData(
            Dictionary<string, Commit> commits,
            Dictionary<string, string> remotes,
            Dictionary<string, Commit> branches,
            Dictionary<string, Commit> tags)
        {
            _commits = commits;
            _remotes = remotes;
            _branches = branches;
            _tags = tags;
        }
    }
}