using Prigitsk.Core.Tools;
using Prigitsk.Core.Tree;

namespace Prigitsk.Core.Rendering
{
    public interface ITreeRenderer
    {
        void Render(ITree tree, ITextWriter textWriter, ITreeRenderingOptions options);
    }
}