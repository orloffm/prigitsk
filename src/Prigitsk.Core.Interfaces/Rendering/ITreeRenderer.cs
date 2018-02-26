using Prigitsk.Core.Entities;
using Prigitsk.Core.Strategy;
using Prigitsk.Core.Tools;
using Prigitsk.Core.Tree;

namespace Prigitsk.Core.Rendering
{
    public interface ITreeRenderer
    {
        void Render(
            ITree tree,
            ITextWriter textWriter,
            IRemote usedRemote,
            IBranchingStrategy strategy,
            ITreeRenderingOptions options);
    }
}