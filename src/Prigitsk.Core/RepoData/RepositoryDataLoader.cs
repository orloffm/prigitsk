using System.Linq;
using Prigitsk.Core.Git;

namespace Prigitsk.Core.RepoData
{
    public class RepositoryDataLoader : IRepositoryDataLoader
    {
        private readonly IRepositoryDataBuilderFactory _dataBuilderFactory;
        private readonly IGitRepositoryFactory _repositoryFactory;

        public RepositoryDataLoader(
            IGitRepositoryFactory repositoryFactory,
            IRepositoryDataBuilderFactory dataBuilderFactory)
        {
            _repositoryFactory = repositoryFactory;
            _dataBuilderFactory = dataBuilderFactory;
        }

        public IRepositoryData LoadFrom(string gitRepository)
        {
            IRepositoryDataBuilder builder = _dataBuilderFactory.CreateBuilder();

            using (IGitRepository repository = _repositoryFactory.Open(gitRepository))
            {
                PopulateBuilder(builder, repository);
            }

            IRepositoryData data = builder.Build();
            return data;
        }

        private void PopulateBuilder(IRepositoryDataBuilder builder, IGitRepository repo)
        {
            // Commits
            foreach (IGitCommit c in repo.Commits)
            {
                string[] parentShas = c.Parents.Select(p => p.Sha).ToArray();

                builder.AddCommit(c.Sha, parentShas, c.Committer.When);
            }

            // Remotes
            foreach (IGitRemote r in repo.Remotes)
            {
                builder.AddRemote(r.Name, r.Url);
            }

            // Branches
            foreach (IGitBranch b in repo.Branches)
            {
                if (!b.IsRemote)
                {
                    continue;
                }

                builder.AddRemoteBranch(b.FriendlyName, b.Tip.Sha);
            }

            // Tags
            foreach (IGitTag t in repo.TagsOnCommits)
            {
                builder.AddTag(t.FriendlyName, t.Tip.Sha);
            }
        }
    }
}