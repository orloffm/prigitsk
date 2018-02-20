using Microsoft.Extensions.Logging;
using Prigitsk.Core.Tools;
using Prigitsk.Core.Tree;

namespace Prigitsk.Core.Rendering
{
    public sealed class TreeRenderer : ITreeRenderer
    {
        private readonly ILogger _log;

        public TreeRenderer(ILogger log)
        {
            _log = log;
        }

        public void Render(ITree tree, ITextWriter textWriter, ITreeRenderingOptions options)
        {
            textWriter.WriteLine("That's all folks!");
        }
    }
}