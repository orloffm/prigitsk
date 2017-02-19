namespace OrlovMikhail.GitTools.Loading
{
    public interface IRepositoryDataLoader
    {
        IRepositoryData Load(string repositoryPath);
    }
}