using System;
using Prigitsk.Core.RepoData;

namespace Prigitsk.Core.Graph.Making
{
    [Obsolete]
    public interface IBranchAssumer
    {
        IAssumedGraph AssumeTheBranchGraph(IRepositoryData repositoryData);
    }
}