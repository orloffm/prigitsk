using Prigitsk.Core.Entities;
using Prigitsk.Core.Strategy;
using Prigitsk.Core.Tree;

namespace Prigitsk.Core.Rendering
{
    public interface ITreeRenderer
    {
        void Render(
            ITree tree,
            IRemote usedRemote,
            IBranchingStrategy strategy,
            ITreeRenderingOptions options);
    }
}