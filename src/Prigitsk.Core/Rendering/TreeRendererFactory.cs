using System;
using OrlovMikhail.GraphViz.Writing;

namespace Prigitsk.Core.Rendering
{
    public class TreeRendererFactory : ITreeRendererFactory
    {
        private readonly Func<IGraphVizWriter, ITreeRenderer> _rendererMaker;

        public TreeRendererFactory(Func<IGraphVizWriter, ITreeRenderer> rendererMaker)
        {
            _rendererMaker = rendererMaker;
        }

        public ITreeRenderer CreateRenderer(IGraphVizWriter textWriter)
        {
            return _rendererMaker(textWriter);
        }
    }
}