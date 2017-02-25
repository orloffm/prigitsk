using OrlovMikhail.GitTools.Loading.Client.Repository;
using OrlovMikhail.GitTools.Structure;

namespace OrlovMikhail.GitTools.Processing
{
    public interface IRepositoryProcessor
    {
        IProcessedRepository Process(IRepositoryState state,  RepositoryProcessingOptions options);
    }
}