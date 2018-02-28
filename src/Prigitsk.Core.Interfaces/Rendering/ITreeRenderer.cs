using Prigitsk.Core.Entities;
using Prigitsk.Core.Graph;
using Prigitsk.Core.Strategy;

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