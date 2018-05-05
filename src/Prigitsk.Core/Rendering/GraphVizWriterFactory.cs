using System;
using OrlovMikhail.GraphViz.Writing;
using Thinktecture.IO;

namespace Prigitsk.Core.Rendering
{
    public class GraphVizWriterFactory : IGraphVizWriterFactory
    {
        private readonly Func<ITextWriter, IGraphVizWriter> _rendererMaker;

        public GraphVizWriterFactory(Func<ITextWriter, IGraphVizWriter> rendererMaker)
        {
            _rendererMaker = rendererMaker;
        }

        public IGraphVizWriter CreateGraphVizWriter(ITextWriter textWriter)
        {
            return _rendererMaker(textWriter);
        }
    }
}