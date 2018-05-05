using OrlovMikhail.GraphViz.Writing;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Rendering
{
    public static class GraphVizWriterTreeishExtensions
    {
        public static void Edge(this IGraphVizWriter writer, ITreeish a, ITreeish b, IAttrSet attrSet = null)
        {
            writer.Edge(a.Treeish, b.Treeish, attrSet);
        }

        public static void Node(this IGraphVizWriter writer, ITreeish n, IAttrSet attrSet = null)
        {
            writer.Node(n.Treeish, attrSet);
        }
    }
}