using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging;
using OrlovMikhail.GraphViz.Writing;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Graph;
using Prigitsk.Core.Remotes;
using Prigitsk.Core.Strategy;
using Prigitsk.Framework;
using Thinktecture.IO;

namespace Prigitsk.Core.Rendering
{
    public sealed class TreeRenderer2 : ITreeRenderer
    {
        private const string BranchEdgesFormat
            = @"edge[color=""{0}"", penwidth={1}];";

        private const string BranchNodesFormat
            = @"node[group=""{0}"", fillcolor=""{1}"", color=""{2}""];";

        private readonly ILogger _log;
        private readonly IRemoteWebUrlProviderFactory _remoteWebUrlProviderFactory;
        private readonly IGraphVizWriter _gvWriter;

        public TreeRenderer2(
            ILogger<TreeRenderer2> log,
            IGraphVizWriter gvWriter,
            IRemoteWebUrlProviderFactory remoteWebUrlProviderFactory)
        {
            _log = log;
            _gvWriter = gvWriter;
            _remoteWebUrlProviderFactory = remoteWebUrlProviderFactory;
        }

        public void Render(
            ITree tree,
            IRemote usedRemote,
            IBranchingStrategy branchingStrategy,
            ITreeRenderingOptions options)
        {
            IRemoteWebUrlProvider remoteUrlProvider =
                _remoteWebUrlProviderFactory.CreateUrlProvider(usedRemote.Url, options.ForceTreatAsGitHub);

            WriteHeader();

            IBranch[] currentBranches = tree.Branches.Where(b => tree.EnumerateNodes(b).Any()).ToArray();
            WriteCurrentBranchesLabels(currentBranches, remoteUrlProvider);

            // Tags and orphaned branches names,
            ITag[] tags = tree.Tags.ToArray();

            IBranch[] orphanedBranches = tree.Branches.Where(b => !tree.EnumerateNodes(b).Any()).ToArray();

            WriteTagsAndOrphanedBranches(tags, orphanedBranches, remoteUrlProvider);

            // Now the graph itself.
            var otherLinks = new PairList<INode, INode>();
            WriteNodes(tree, branchingStrategy, currentBranches, remoteUrlProvider, otherLinks);

            // Render all other edges
            WriteOtherEdges(otherLinks, remoteUrlProvider);

            // Tags and orphaned branches
            WriteTagsAndOrphanedBranchesConnections(tags, orphanedBranches);
            WriteFooter();
        }

        private string MakeNodeHandle(INode node)
        {
            return MakeNodeHandle(node.Commit.Hash);
        }

        private string MakeNodeHandle(IHash hash)
        {
            return hash.ToShortString();
        }

        private string MakePointerHandle(IPointer pointerObject)
        {
            string pointerLabel = pointerObject.Label
                .Trim()
                .Replace(".", "_")
                .Replace("-", "_")
                .Replace(" ", "_");

            return pointerLabel;
        }

        private void WriteCurrentBranchesLabels(
            IEnumerable<IBranch> currentBranches,
            IRemoteWebUrlProvider remoteUrlProvider)
        {
            _gvWriter.Comment("Branch names.");

            _gvWriter.WriteNodeAttributes(
                AttrSet.Empty
                    .Shape(Shape.None)
                    .FixedSize(false)
                    .PenWidth(0)
                    .FillColor(GraphVizColor.None)
                    .Width(0)
                    .Height(0)
                    .Margin(0.05)
            );

            _gvWriter.WriteLine(
                @"// branch names
    node[shape = none, fixedsize = false, penwidth = O, fillcolor = none, width = 0, height = 0, margin =""0.05""];");
            // 0 - branch short name
            // 1 - branch label
            // 2 - repository path
            const string currentBranchLabelFormat = @"subgraph {{
rank = sink;
    ""{0}"" [label=""{1}"", group=""{0}"", URL=""{2}""];
}}";

            foreach (IBranch b in currentBranches)
            {
                string url = remoteUrlProvider?.GetBranchLink(b);

                string text = string.Format(
                    currentBranchLabelFormat,
                    MakePointerHandle(b),
                    b.Label,
                    url);

                _gvWriter.Node(
                    b.Label,
                    AttrSet.Empty
                        .Label(b.Label)
                        .Group(b.Label)
                        .Url(url)
                );

                _gvWriter.WriteLine(text);
            }
        }


        private void WriteFooter()
        {
            _gvWriter.EndGraph();
        }

        private void WriteHeader()
        {
            _gvWriter.StartGraph(GraphMode.Digraph, strict: true);

            _gvWriter.WriteGraphAttributes(
                AttrSet.Empty
                    .Rankdir(Rankdir.LR)
                    .NodeSep(0.2)
                    .RankSep(0.1)
                    .ForceLabels(false));



    //        _gvWriter.WriteLine(
    //            @"strict digraph g{
    //rankdir = ""LR"";
    //nodesep = 0.2;
    //ranksep = 0.25;
    //// splines=line;
    //forcelabels = false;
    //// general
    //graph[fontname = ""Consolas"", fontsize = ""16pt"", fontcolor = ""black""];
    //node[fontname = ""Consolas"", fontsize ="" 16pt"", fontcolor = ""black""];
    //edge[fontname = ""Consolas"", fontsize ="" 16pt"", fontcolor = ""black""];
    //node[style = filled, color = ""black""];
    //edge[arrowhead = vee, color = ""black"", penwidth = l];
    //");
        }

        private void WriteInBranchEdge(
            INode parentNode,
            INode childNote)
        {
            const string edgeFormatSimple = @"""{0}"" -> ""{1}""; ";
            const string edgeFormatDotted = @"""{0}"" -> ""{1}"" [style=dashed];";

            // We draw it dotted.
            string formatToUse = childNote.AbsorbedParentCommits.Any() ? edgeFormatDotted : edgeFormatSimple;
            _gvWriter.WriteLine(string.Format(formatToUse, MakeNodeHandle(parentNode), MakeNodeHandle(childNote)));
        }

        private void WriteNode(INode n, IRemoteWebUrlProvider remoteUrlProvider)
        {
            double width = 0.2d;
            string size = width.ToString("##.##", CultureInfo.InvariantCulture);
            const string nodeFormat = @"""{0}"" [width={1}, height={1}, URL=""{2}""];";
            string url = remoteUrlProvider?.GetCommitLink(n.Commit);
            _gvWriter.WriteLine(string.Format(nodeFormat, MakeNodeHandle(n), size, url));
        }

        private void WriteNodes(
            ITree tree,
            IBranchingStrategy branchingStrategy,
            IBranch[] currentBranches,
            IRemoteWebUrlProvider remoteUrlProvider,
            PairList<INode, INode> otherLinks)
        {
            _gvWriter.WriteLine(
                @"// graph
node[width = 0.2, height = 0.2, fixedsize = true, label ="""", margin=""0.11, 0.055"", shape = circle, penwidth = 2, fillcolor = ""#FF0000""]
// branches");
            const string branchNodesStart = @"subgraph {";
            const string branchNodesEnd = @"
}";

            var firstNodeDates = new Dictionary<IBranch, DateTimeOffset>();
            foreach (IBranch b in currentBranches)
            {
                DateTimeOffset firstNodeDate = tree.EnumerateNodes(b).First().Commit.CommittedWhen.Value;
                firstNodeDates.Add(b, firstNodeDate);
            }

            IBranch[] currentBranchesSorted =
                branchingStrategy.SortForWritingDescending(currentBranches, firstNodeDates).ToArray();

            foreach (IBranch b in currentBranchesSorted)
            {
                // Header.
                // 0 - branch short name
                // 1 - color
                string htmlString = branchingStrategy.GetHtmlColorFor(b);
                _gvWriter.WriteLine(string.Format(BranchNodesFormat, MakePointerHandle(b), htmlString, htmlString));
                _gvWriter.WriteLine(string.Format(BranchEdgesFormat, htmlString, 2));
                _gvWriter.WriteLine(branchNodesStart);
                // All nodes in the branch.
                INode[] nodesInBranch = tree.EnumerateNodes(b).ToArray();
                var edgesInBranch = new PairList<INode, INode>();
                for (int index = 0; index < nodesInBranch.Length; index++)
                {
                    INode currentNode = nodesInBranch[index];
                    INode nextNode = index < nodesInBranch.Length - 1 ? nodesInBranch[index + 1] : null;
                    WriteNode(currentNode, remoteUrlProvider);
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

                foreach (Tuple<INode, INode> tuple in edgesInBranch.EnumerateItems())
                {
                    WriteInBranchEdge(tuple.Item1, tuple.Item2);
                }

                _gvWriter.WriteLine(branchNodesEnd);
                // Now link to the branch name node.
                // 0 - last node in branch
                // 1 - branch short name
                const string branchEndEdge = @"""{0}"" -> ""{1}"" [color=""#b0b0b0"", style=dotted, arrowhead=none];";
                string text = string.Format(branchEndEdge, MakeNodeHandle(b.Tip), MakePointerHandle(b));
                _gvWriter.WriteLine(text);
                _gvWriter.WriteLine();
            }

            INode[] allLeftOvers = tree.Nodes.Where(n => tree.GetContainingBranch(n) == null).ToArray();
            if (allLeftOvers.Length > 0)
            {
                // Left-overs, without branches.
                _gvWriter.WriteLine(string.Format(BranchNodesFormat, string.Empty, "white", "black"));
                foreach (INode currentNode in allLeftOvers)
                {
                    WriteNode(currentNode, remoteUrlProvider);
                    // Remember children.
                    foreach (INode child in currentNode.Children)
                    {
                        otherLinks.Add(currentNode, child);
                    }
                }
            }
        }

        private void WriteOtherEdges(
            PairList<INode, INode> otherLinks,
            IRemoteWebUrlProvider remoteUrlProvider)
        {
            _gvWriter.WriteLine("// all other edges");
            _gvWriter.WriteLine(string.Format(BranchEdgesFormat, "black", 1));
            foreach (Tuple<INode, INode> pair in otherLinks.EnumerateItems())
            {
                const string edgeFormatUrl = @"""{0}"" -> ""{1}"" [URL = ""{2}""];";

                INode nodeA = pair.Item1;
                INode nodeB = pair.Item2;
                string url = remoteUrlProvider?.GetCompareCommitsLink(nodeA.Commit, nodeB.Commit);
                string text = string.Format(
                    edgeFormatUrl,
                    MakeNodeHandle(nodeA),
                    MakeNodeHandle(nodeB),
                    url);
                _gvWriter.WriteLine(text);
            }

            _gvWriter.WriteLine();
        }

        private void WriteTagsAndOrphanedBranches(
            ITag[] tags,
            IBranch[] orphanedBranches,
            IRemoteWebUrlProvider remoteUrlProvider)
        {
            _gvWriter.WriteLine(@"// orphaned branches");
            // 0 - branch short name
            // 1 - branch friendly name
            // 2 - repository path
            const string orphanedBranchFormat = @"""{0}"" [label=""{1}"", URL=""{2}""];";

            foreach (IBranch b in orphanedBranches)
            {
                string url = remoteUrlProvider?.GetBranchLink(b);
                string text = string.Format(
                    orphanedBranchFormat,
                    MakePointerHandle(b),
                    b.Label,
                    url);
                _gvWriter.WriteLine(text);
            }

            _gvWriter.WriteLine(
                @"// tags
node[shape = cds, fixedsize = false, fillcolor =""#C6C6C6"", penwidth=l, margin=""0.11,0.055""]");
            // 0 - tag short name
            // 1 - tag friendly name
            // 2 - repository path
            const string tagFormat = @"""{0}"" [label=""{1}"", URL=""{2}""];";
            foreach (ITag tag in tags)
            {
                string url = remoteUrlProvider?.GetTagLink(tag);
                string text = string.Format(
                    tagFormat,
                    MakePointerHandle(tag),
                    tag.Label,
                    url);
                _gvWriter.WriteLine(text);
            }
        }

        private void WriteTagsAndOrphanedBranchesConnections(
            IEnumerable<ITag> tags,
            IEnumerable<IBranch> orphanedBranches)
        {
            // orphaned branches are simply linked
            _gvWriter.WriteLine(
                @"// orphaned branches links
        edge[color = ""#b0b0b0"", style=dotted, arrowhead=none, len=0.3];");
            // 0 - source node
            // 1 - branch short name
            const string orphanedBranchLinkFormat = @"""{0}"" -> ""{1}"";";
            foreach (IBranch b in orphanedBranches)
            {
                string text = string.Format(
                    orphanedBranchLinkFormat,
                    MakeNodeHandle(b.Tip),
                    MakePointerHandle(b));
                _gvWriter.WriteLine(text);
            }

            _gvWriter.WriteLine();
            _gvWriter.WriteLine(
                @"// tags
        edge[penwidth = l];");
            // 0 - source node
            // 1 - tag short name
            const string tagLinkFormat = @"subgraph {{
    rank = ""same"";
    ""{0}"" -> ""{1}"";
}}";
            foreach (ITag tag in tags)
            {
                string text = string.Format(
                    tagLinkFormat,
                    MakeNodeHandle(tag.Tip),
                    MakePointerHandle(tag));
                _gvWriter.WriteLine(text);
            }
        }
    }
}