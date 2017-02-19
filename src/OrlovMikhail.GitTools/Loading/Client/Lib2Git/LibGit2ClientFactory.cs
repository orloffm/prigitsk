using OrlovMikhail.GitTools.Loading.Client.Common;

namespace OrlovMikhail.GitTools.Loading
{
    public class LibGit2ClientFactory : IGitClientFactory
    {
        public IGitClient CreateClient(string repositoryPath)
        {
            return new LibGit2Client(repositoryPath);
        }
    }
}