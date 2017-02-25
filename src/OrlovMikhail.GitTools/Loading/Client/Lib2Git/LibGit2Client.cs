using System.Linq;
using LibGit2Sharp;
using OrlovMikhail.GitTools.Loading.Client.Common;
using OrlovMikhail.GitTools.Loading.Client.Repository;

namespace OrlovMikhail.GitTools.Loading
{
    public class LibGit2Client : IGitClient
    {
        private readonly IRepositoryDataBuilderFactory _builderFactory;
        private readonly string _repositoryPath;
        private Repository _repository;

        public LibGit2Client(IRepositoryDataBuilderFactory builderFactory, string repositoryPath)
        {
            _builderFactory = builderFactory;
            _repositoryPath = repositoryPath;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        private string AbbreviateHash(GitObject source)
        {
            return source.Id.Sha.Substring(0, 7);
        }

        public void Init()
        {
            _repository = new Repository(_repositoryPath);
        }

        public IRepositoryData Load(GitClientLoadingOptions? options = null)
        {
            IRepositoryDataBuilder ret = _builderFactory.CreateBuilder();

            foreach (Commit c in _repository.Commits)
            {
                string hash = AbbreviateHash(c);
                string[] parentHashes = c.Parents.Select(AbbreviateHash).ToArray();

                ret.AddCommit(hash, parentHashes, c.Message);
            }

            foreach (Branch b in _repository.Branches)
            {
                if (!b.IsRemote)
                {
                    continue;
                }

                string sourceHash = AbbreviateHash(b.Tip);

                ret.AddRemoteBranch(b.FriendlyName, sourceHash);
            }

            foreach (Tag t in _repository.Tags)
            {
                string sourceHash = AbbreviateHash(t.PeeledTarget);

                ret.AddTag(t.FriendlyName, sourceHash);
            }

            IRepositoryData repositoryData = ret.Build();
            return repositoryData;
        }
    }
}