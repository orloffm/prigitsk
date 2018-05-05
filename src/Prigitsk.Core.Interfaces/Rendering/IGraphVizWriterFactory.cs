using OrlovMikhail.GraphViz.Writing;
using Thinktecture.IO;

namespace Prigitsk.Core.Rendering
{
    public interface IGraphVizWriterFactory
    {
        IGraphVizWriter CreateGraphVizWriter(ITextWriter textWriter);
    }
}