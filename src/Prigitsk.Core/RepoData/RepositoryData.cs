using System.Collections.Generic;
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
            Commits = new CommitsData(commits);
            Remotes = new RemotesData(remotes);
            Branches = new BranchesData(branches);
            Tags = new TagsData(tags);
        }

        public IBranchesData Branches { get; }

        public ICommitsData Commits { get; }

        public IRemotesData Remotes { get; }

        public ITagsData Tags { get; }
    }
}