using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Prigitsk.Core.Graph.Strategy;
using Prigitsk.Core.Nodes;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Graph.Writing
{
    public class TreeWriter : ITreeWriter
    {
        private const string branchNodesFormat
            = @"node [group={0}, fillcolor=""{1}"", color=""{2}""];";

        private const string branchEdgesFormat
            = @"edge[color = ""{0}"", penwidth={1}];";

        private readonly string _repositoryPath;
        private readonly CultureInfo _ukCulture;
        private readonly INodeWeightInformer _weightInformer;

        public TreeWriter(string repositoryPath)
        {
            _repositoryPath = repositoryPath;
            _weightInformer = new NodeWeightInformer();
            _weightInformer.MinWidth = 0.2d;
            _weightInformer.MaxWidth = 0.5d;
            _ukCulture = new CultureInfo("en-gb");
        }

        public string GenerateGraph(IAssumedGraph graph, IBranchingStrategy branchingStrategy)
        {
            StringBuilder sb = new StringBuilder();
            WriteHeader(sb);
            // The graph nodes with current branch names,
            OriginBranch[] currentBranches = graph.GetCurrentBranches();
            WriteCurrentBranchesLabels(sb, currentBranches);
            // Tags and orphaned branches names,
            Tag[] tags = graph.GetAllTags();
            OriginBranch[] orphanedBranches = graph.GetOrphanedBranches();
            WriteTagsAndOrphanedBranches(sb, tags, orphanedBranches);
            // Now the graph itself.
            var otherLinks = new PairList<Node, Node>();
            WriteNodes(graph, branchingStrategy, sb, currentBranches, otherLinks);
            // Write all other edges
            WriteOtherEdges(otherLinks, graph, sb);
            // Tags and orphaned branches
            WriteTagsAndOrphanedBranchesConnections(sb, tags, orphanedBranches);
            WriteFooter(sb);
            string result = sb.ToString();
            return result;
        }

        private string MakeHandle(Pointer pointerObject)
        {
            string pointerLabel = pointerObject.Label.Trim().Replace(".", "_")
                .Replace("-", "_").Replace(" ", "_");

            return @"""" + pointerLabel + @"""";
        }

        private string MakeNodeHandle(Node node)
        {
            return @"""" + node.Hash + @"""";
        }

        private void WriteInBranchEdge(StringBuilder sb, Node parentNode, Node childNote)
        {
            const string edgeFormatSimple = @"{0} -> {1}; ";
            const string edgeFormatDotted = @"{0} -> {1} [style=dashed];";

            // We draw it dotted.
            string formatToUse = childNote.SomethingWasMergedInto ? edgeFormatDotted : edgeFormatSimple;
            sb.AppendLine(string.Format(formatToUse, MakeNodeHandle(parentNode), MakeNodeHandle(childNote)));
        }

        private void WriteCurrentBranchesLabels(
            StringBuilder sb,
            OriginBranch[] currentBranches)
        {
            sb.AppendLine(
                @"// branch names
    node[shape = none, fixedsize = false, penwidth = O, fillcolor = none, width = 0, height = 0, margin =""0.05""];");
            // 0 - branch short name
            // 1 - branch label
            // 2 - repository path
            const string currentBranchLabelFormat = @"subgraph {{
rank = sink;
    {0} [label=""{1}"", group={0}, URL=""{2}/tree/{1}""];
}}";
            foreach (OriginBranch b in currentBranches)
            {
                string text = string.Format(
                    currentBranchLabelFormat,
                    MakeHandle(b),
                    b.Label,
                    _repositoryPath);
                sb.AppendLine(text);
            }
        }

        private void WriteFooter(StringBuilder sb)
        {
            sb.Append("}");
        }

        private void WriteHeader(StringBuilder sb)
        {
            sb.AppendLine(
                @"strict digraph g{
    rankdir = ""LR"";
    nodesep = 0.2;
    ranksep = 0.25;
    // splines=line;
    forcelabels = false;
    // general
    graph[fontname = ""Consolas"", fontsize = ""16pt"", fontcolor = ""black""];
    node[fontname = ""Consolas"", fontsize ="" 16pt"", fontcolor = ""black""];
    edge[fontname = ""Consolas"", fontsize ="" 16pt"", fontcolor = ""black""];
    node[style = filled, color = ""black""];
    edge[arrowhead = vee, color = ""black"", penwidth = l];
    ");
        }

        private void WriteNode(StringBuilder sb, Node n)
        {
            double width = _weightInformer.GetWidth(n);
            string size = width.ToString("##.##", _ukCulture);
            const string nodeFormat = @"{0} [width={1}, height={1}, URL=""{3}/commit/{2}""];";
            sb.AppendLine(string.Format(nodeFormat, MakeNodeHandle(n), size, n.Hash, _repositoryPath));
        }

        private void WriteNodes(
            IAssumedGraph graph,
            IBranchingStrategy branchingStrategy,
            StringBuilder sb,
            OriginBranch[] currentBranches,
            PairList<Node, Node> otherLinks)
        {
            // Initialize weights.
            _weightInformer.Init(graph.EnumerateAllContainedNodes());
            sb.AppendLine(
                @"// graph
node[width = 0.2, height = 0.2, fixedsize = true, label ="""", margin=""0.11, 0.055"", shape = circle, penwidth = 2, fillcolor = ""#FF0000""]
// branches");
            const string branchNodesStart = @"subgraph {";
            const string branchNodesEnd = @"
}";

            var firstNodeDates = new Dictionary<OriginBranch, DateTime>();
            foreach (OriginBranch b in currentBranches)
            {
                DateTime firstNodeDate = graph.GetFirstNodeDate(b);
                firstNodeDates.Add(b, firstNodeDate);
            }
            OriginBranch[] currentBranchesSorted =
                branchingStrategy.SortForWriting(currentBranches, firstNodeDates).ToArray();
            foreach (OriginBranch b in currentBranchesSorted)
            {
                // Header.
                // 0 - branch short name
                // 1 - color
                string htmlString = branchingStrategy.GetHTMLColorFor(b);
                sb.AppendLine(string.Format(branchNodesFormat, MakeHandle(b), htmlString, htmlString));
                sb.AppendLine(string.Format(branchEdgesFormat, htmlString, 2));
                sb.AppendLine(branchNodesStart);
                // All nodes in the branch.
                Node[] nodesInBranch = graph.GetNodesConsecutive(b);
                var edgesInBranch = new PairList<Node, Node>();
                for (int index = 0; index < nodesInBranch.Length; index++)
                {
                    Node currentNode = nodesInBranch[index];
                    Node nextNode = index < nodesInBranch.Length - 1 ? nodesInBranch[index + 1] : null;
                    WriteNode(sb, currentNode);
                    if (nextNode != null)
                    {
                        edgesInBranch.Add(currentNode, nextNode);
                    }
                    // Other children.
                    foreach (Node child in currentNode.Children)
                    {
                        otherLinks.Add(currentNode, child);
                    }
                }
                foreach (Tuple<Node, Node> tuple in edgesInBranch.EnumerateItems())
                {
                    WriteInBranchEdge(sb, tuple.Item1, tuple.Item2);
                }
                sb.AppendLine(branchNodesEnd);
                // Now link to the branch name node.
                // 0 - last node in branch
                // 1 - branch short name
                const string branchEndEdge = @"{0} -> {1}	[color=""#b0b0b0"", style=dotted, arrowhead=none];";
                string text = string.Format(branchEndEdge, MakeNodeHandle(b.Source), MakeHandle(b));
                sb.AppendLine(text);
                sb.AppendLine();
            }
            Node[] allLeftOvers = graph.EnumerateAllLeftOvers().ToArray();
            if (allLeftOvers.Length > 0)
            {
                // Left-overs, without branches.
                sb.AppendLine(string.Format(branchNodesFormat, @"""""", "white", "black"));
                foreach (Node currentNode in allLeftOvers)
                {
                    WriteNode(sb, currentNode);
                    // Remember children.
                    foreach (Node child in currentNode.Children)
                    {
                        otherLinks.Add(currentNode, child);
                    }
                }
            }
        }

        private void WriteOtherEdges(
            PairList<Node, Node> otherLinks,
            IAssumedGraph graph,
            StringBuilder sb)
        {
            sb.AppendLine("// all other edges");
            sb.AppendLine(string.Format(branchEdgesFormat, "black", 1));
            foreach (Tuple<Node, Node> pair in otherLinks.EnumerateItems())
            {
                Node nodeA = pair.Item1;
                Node nodeB = pair.Item2;
                //var branchA = graph.GetBranch(nodeA);
                //var branchB = graph.GetBranch(nodeB);
                string text;
                // 0 - node A
                // 1 - node B
                // 2 - repository path
                const string edgeFormatUrl = @"{0} -> {1} [URL = ""{2}/compare/{3}...{4}""];";
                text = string.Format(
                    edgeFormatUrl,
                    MakeNodeHandle(nodeA),
                    MakeNodeHandle(nodeB),
                    _repositoryPath,
                    nodeA.Hash,
                    nodeB.Hash);
                sb.AppendLine(text);
            }
            sb.AppendLine();
        }

        private void WriteTagsAndOrphanedBranches(
            StringBuilder sb,
            Tag[] tags,
            OriginBranch[] orphanedBranches)
        {
            sb.AppendLine(@"// orphaned branches");
            // 0 - branch short name
            // 1 - branch friendly name
            // 2 - repository path
            const string orphanedBranchFormat = @"{0} [label=""{1}"", URL=""{2}/tree/{1}""];";
            foreach (OriginBranch b in orphanedBranches)
            {
                string text = string.Format(
                    orphanedBranchFormat,
                    MakeHandle(b),
                    b.Label,
                    _repositoryPath);
                sb.AppendLine(text);
            }
            sb.AppendLine(
                @"// tags
node[shape = cds, fixedsize = false, fillcolor =""#C6C6C6"", penwidth=l, margin=""0.11,0.055""]");
            // 0 - tag short name
            // 1 - tag friendly name
            // 2 - repository path
            const string tagFormat = @"{0}	[label=""{1}"", URL=""{2}/releases/tag/{1}""];";
            foreach (Tag tag in tags)
            {
                string text = string.Format(
                    tagFormat,
                    MakeHandle(tag),
                    tag.Label,
                    _repositoryPath);
                sb.AppendLine(text);
            }
        }

        private void WriteTagsAndOrphanedBranchesConnections(
            StringBuilder sb,
            Tag[] tags,
            OriginBranch[] orphanedBranches)
        {
            // orphaned branches are simply linked
            sb.AppendLine(
                @"// orphaned branches links
        edge[color = ""#b0b0b0"", style=dotted, arrowhead=none, len=0.3];");
            // 0 - source node
            // 1 - branch short name
            const string orphanedBranchLinkFormat = @"{0} -> {1};";
            foreach (OriginBranch b in orphanedBranches)
            {
                string text = string.Format(
                    orphanedBranchLinkFormat,
                    MakeNodeHandle(b.Source),
                    MakeHandle(b));
                sb.AppendLine(text);
            }
            sb.AppendLine();
            sb.AppendLine(
                @"// tags
        edge[penwidth = l];");
            // 0 - source node
            // 1 - tag short name
            const string tagLinkFormat = @"subgraph {{
    rank = ""same"";
    {0} -> {1};
}}";
            foreach (Tag tag in tags)
            {
                string text = string.Format(
                    tagLinkFormat,
                    MakeNodeHandle(tag.Source),
                    MakeHandle(tag));
                sb.AppendLine(text);
            }
        }
    }
}