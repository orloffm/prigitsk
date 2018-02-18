using System.Linq;
using Prigitsk.Core.Git;
using Prigitsk.Core.Nodes.Loading;

namespace Prigitsk.Core.RepoData
{
    public class RepositoryDataLoader : IRepositoryDataLoader
    {
        private readonly IRepositoryDataBuilderFactory _dataBuilderFactory;
        private readonly IRepositoryFactory _repositoryFactory;

        public RepositoryDataLoader(
            IRepositoryFactory repositoryFactory,
            IRepositoryDataBuilderFactory dataBuilderFactory)
        {
            _repositoryFactory = repositoryFactory;
            _dataBuilderFactory = dataBuilderFactory;
        }

        public IRepositoryData LoadFrom(string gitRepository)
        {
            IRepositoryDataBuilder builder = _dataBuilderFactory.CreateBuilder();

            using (IRepository repository = _repositoryFactory.Open(gitRepository))
            {
                PopulateBuilder(builder, repository);
            }

            IRepositoryData data = builder.Build();
            return data;
        }

        private void PopulateBuilder(IRepositoryDataBuilder builder, IRepository repo)
        {
            // Commits
            foreach (ICommit c in repo.Commits)
            {
                string[] parentShas = c.Parents.Select(p => p.Sha).ToArray();

                builder.AddCommit(c.Sha, parentShas, c.Committer.When);
            }

            // Remotes
            foreach (IRemote r in repo.Remotes)
            {
                builder.AddRemote(r.Name, r.Url);
            }

            // Branches
            foreach (IBranch b in repo.Branches)
            {
                if (!b.IsRemote)
                {
                    continue;
                }

                builder.AddRemoteBranch(b.FriendlyName, b.Tip.Sha);
            }

            // Tags
            foreach (ITag t in repo.TagsOnCommits)
            {
                builder.AddTag(t.FriendlyName, t.Tip.Sha);
            }
        }
    }
}