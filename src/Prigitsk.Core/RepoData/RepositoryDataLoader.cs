using System;
using System.Linq;
using Prigitsk.Core.Git;
using Prigitsk.Core.Git.LibGit2Sharp;

namespace Prigitsk.Core.Nodes.Loading
{
    public class RepositoryDataLoader : IRepositoryDataLoader
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IRepositoryDataBuilderFactory _dataBuilderFactory;

        public RepositoryDataLoader(IRepositoryFactory repositoryFactory, IRepositoryDataBuilderFactory dataBuilderFactory)
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

            var data = builder.Build();
            return data;
        }

        private void PopulateBuilder(IRepositoryDataBuilder builder, IRepository repo)
        {
            // Commits
            foreach (var c in repo.Commits)
            {
                string[] parentShas = c.Parents.Select(p => p.Sha).ToArray();

                builder.AddCommit(c.Sha, parentShas, c.Committer.When);
            }

            // Remotes
            foreach (var r in repo.Remotes)
            {
                builder.AddRemote(r.Name, r.Url);
            }

            // Branches
            foreach (var b in repo.Branches)
            {
                if (!b.IsRemote)
                {
                    continue;
                }

                builder.AddBranch(b.FriendlyName, b.Tip.Sha);
            }

            // Tags
            foreach (var t in repo.TagsOnCommits)
            {
                builder.AddTag(t.FriendlyName, t.Tip.Sha);
            }

        }
    }
}