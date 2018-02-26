using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Rendering
{
    public interface ITreeRendererFactory
    {
        ITreeRenderer CreateRenderer(ITextWriter textWriter);
    }
}