using System;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Graph;

namespace Prigitsk.Core.Rendering
{
    public sealed class GraphTooltipHelper : IGraphTooltipHelper
    {
        public string MakeNodeTooltip(INode node)
        {
            string MakeSignature(ISignature whom, string prefix = "")
            {
                return $"{Environment.NewLine}{prefix}{whom.Name} @ {whom.When})";
            }

            string tooltip = $"{node.Treeish} - {node.Commit.Message}";
            tooltip += MakeSignature(node.Commit.Author);
            if (node.Commit.Author.Name != node.Commit.Committer.Name)
            {
                tooltip += MakeSignature(node.Commit.Committer, "Committed by: ");
            }

            return tooltip;
        }

        public string MakeEdgeTooltip(INode start, INode end)
        {
            return $"{start.Commit.Treeish} -> {end.Commit.Treeish}";
        }
    }
}