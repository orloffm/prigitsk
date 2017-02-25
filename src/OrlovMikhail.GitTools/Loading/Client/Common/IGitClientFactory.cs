namespace OrlovMikhail.GitTools.Loading.Client.Common
{
    public interface IGitClientFactory
    {
        IGitClient CreateClient(string repositoryPath, string gitExePath);
    }
}