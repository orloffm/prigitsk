using Prigitsk.Core.RepoData;

namespace Prigitsk.Core.Graph.Making
{
    public interface IBranchAssumer
    {
        IAssumedGraph AssumeTheBranchGraph(IRepositoryData repositoryData);
    }
}