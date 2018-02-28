using System;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Rendering
{
    public class TreeRendererFactory : ITreeRendererFactory
    {
        private readonly Func<ITextWriter, ITreeRenderer> _rendererMaker;

        public TreeRendererFactory(Func<ITextWriter, ITreeRenderer> rendererMaker)
        {
            _rendererMaker = rendererMaker;
        }

        public ITreeRenderer CreateRenderer(ITextWriter textWriter)
        {
            return _rendererMaker(textWriter);
        }
    }
}