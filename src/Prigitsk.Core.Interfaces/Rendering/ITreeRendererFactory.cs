using OrlovMikhail.GraphViz.Writing;

namespace Prigitsk.Core.Rendering
{
    public interface ITreeRendererFactory
    {
        ITreeRenderer CreateRenderer(IGraphVizWriter textWriter);
    }
}