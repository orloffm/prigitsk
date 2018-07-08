using Prigitsk.Core.Entities;
using Prigitsk.Core.RepoData;
using Prigitsk.Core.Strategy;

namespace Prigitsk.Core.Graph
{
    public interface ITreeBuilder
    {
        ITree Build(
            IRepositoryData repository,
            IRemote remoteToUse,
            IBranchesKnowledge branchesKnowledge,
            ITreeBuildingOptions options);
    }
}