using Prigitsk.Core.Graph;

namespace Prigitsk.Core.Rendering
{
    public interface IGraphTooltipHelper
    {
        string MakeNodeTooltip(INode node);

        string MakeEdgeTooltip(INode node, INode node1);
    }
}