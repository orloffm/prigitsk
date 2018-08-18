using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Graph;

namespace Prigitsk.Core.Rendering
{
    public sealed class GraphTooltipHelper : IGraphTooltipHelper
    {
        public string MakeEdgeTooltip(INode start, INode end)
        {
            int absorbedCommitsCount = end.AbsorbedParentCommits?.Count() ?? 0;
            if(absorbedCommitsCount == 0)
            return $"{start.Commit.Treeish} -> {end.Commit.Treeish}";
            else
                return $"{start.Commit.Treeish} -> ({absorbedCommitsCount} more commits), {end.Commit.Treeish}";
        }

        public string MakeNodeTooltip(INode node)
        {
            string MakeSignature(ISignature whom, string prefix = "")
            {
                return $" // {prefix}{whom.Name} @ {whom.When}";
            }

            string tooltip = $"{node.Treeish} - {node.Commit.Message}";
            tooltip += MakeSignature(node.Commit.Author);
            if (node.Commit.Author.Name != node.Commit.Committer.Name)
            {
                tooltip += MakeSignature(node.Commit.Committer, "Committed by: ");
            }

            return tooltip;
        }
    }
}