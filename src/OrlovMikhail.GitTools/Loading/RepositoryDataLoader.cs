using System.Net.Http.Headers;
using OrlovMikhail.GitTools.Loading.Client.Common;

namespace OrlovMikhail.GitTools.Loading
{
    public   class RepositoryDataLoader : IRepositoryDataLoader
    {
        private readonly IGitClientFactory _gitClientFactory;

        public RepositoryDataLoader(IGitClientFactory gitClientFactory)
        {
            _gitClientFactory = gitClientFactory;
        }

        public IRepositoryData Load(string repositoryPath)
        {
            IRepositoryData ret = new RepositoryData();

            using (IGitClient client = _gitClientFactory.CreateClient(repositoryPath))
            {
                client.Initialise();

                foreach (Commit m in client.Commits)
                {
                    ret.AddCommit(m.Hash, m.Description);
                    foreach (string parentHash in m.ParentHashes)
                        ret.AddParentRelation(parentHash, m.Hash);
                }

                foreach (Branch b in client.Branches)
                {
                    ret.AddBranch(b.Name, b.TargetCommitHash);
                }

                foreach (Tag t in client.Tags)
                {
                    ret.AddTag(t.Name, t.TargetCommitHash);
                }
            }
            
            return ret;
        }
    }
}