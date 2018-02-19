using Prigitsk.Core.RepoData;
using Prigitsk.Core.Strategy;

namespace Prigitsk.Core.Tree
{
    public interface ITreeBuilder
    {
        ITree Build(IRepositoryData repository, IBranchingStrategy strategy, ITreeBuildingOptions options);
    }
}