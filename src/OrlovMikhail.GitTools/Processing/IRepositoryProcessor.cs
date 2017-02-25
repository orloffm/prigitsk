using OrlovMikhail.GitTools.Loading.Client.Repository;

namespace OrlovMikhail.GitTools.Processing
{
    public interface IRepositoryProcessor
    {
        IGraphState Process(IRepositoryState state, RepositoryProcessingOptions options);
    }
}