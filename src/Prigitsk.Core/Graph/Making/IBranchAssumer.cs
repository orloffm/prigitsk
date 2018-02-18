using Prigitsk.Core.Nodes.Loading;

namespace Prigitsk.Core.Graph.Making
{
    public interface IBranchAssumer
    {
        IAssumedGraph AssumeTheBranchGraph(IRepositoryData repositoryData);
    }
}