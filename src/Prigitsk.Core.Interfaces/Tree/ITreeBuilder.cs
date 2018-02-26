using Prigitsk.Core.Entities;
using Prigitsk.Core.RepoData;
using Prigitsk.Core.Strategy;

namespace Prigitsk.Core.Tree
{
    public interface ITreeBuilder
    {
        ITree Build(
            IRepositoryData repository,
            IRemote remoteToUse,
            IBranchingStrategy strategy,
            ITreeBuildingOptions options);
    }
}