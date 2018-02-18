using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.RepoData
{
    public class RepositoryData : IRepositoryData
    {
        public RepositoryData(
            IEnumerable<ICommit> commits,
            IEnumerable<IRemote> remotes,
            IEnumerable<IBranch> branches,
            IEnumerable<ITag> tags)
        {
            Commits = commits.ToArray();
            Remotes = remotes.ToArray();
            Branches = branches.ToArray();
            Tags = tags.ToArray();
        }

        public ICommit[] Commits { get; }
        public IRemote[] Remotes { get; }
        public IBranch[] Branches { get; }
        public ITag[] Tags { get; }
    }
}