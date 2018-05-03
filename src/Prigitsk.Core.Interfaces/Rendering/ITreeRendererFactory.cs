using Thinktecture.IO;

namespace Prigitsk.Core.Rendering
{
    public interface ITreeRendererFactory
    {
        ITreeRenderer CreateRenderer(ITextWriter textWriter);
    }
}