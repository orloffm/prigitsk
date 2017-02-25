using OrlovMikhail.GitTools.Loading.Client.Common;
using OrlovMikhail.GitTools.Loading.Client.Repository;

namespace OrlovMikhail.GitTools.Loading
{
    public class LibGit2ClientFactory : IGitClientFactory
    {
        private readonly IRepositoryDataBuilderFactory _builderFactory;

        public LibGit2ClientFactory(IRepositoryDataBuilderFactory builderFactory)
        {
            _builderFactory = builderFactory;
        }

        public IGitClient CreateClient(string repositoryPath)
        {
            return new LibGit2Client(_builderFactory, repositoryPath);
        }
    }
}