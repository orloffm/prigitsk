using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.Extensions.Logging;
using OrlovMikhail.GraphViz.Writing;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Graph;
using Prigitsk.Core.Remotes;
using Prigitsk.Core.Strategy;
using Prigitsk.Framework;

namespace Prigitsk.Core.Rendering
{
    public sealed class TreeRenderer : ITreeRenderer
    {
        private readonly IGraphVizWriter _gvWriter;
        private readonly ILesserBranchSelectorFactory _lesserBranchSelectorFactory;
        private readonly ILogger _log;
        private readonly IRemoteWebUrlProviderFactory _remoteWebUrlProviderFactory;

        public TreeRenderer(
            ILogger<TreeRenderer> log,
            IGraphVizWriter gvWriter,
            ILesserBranchSelectorFactory lesserBranchSelectorFactory,
            IRemoteWebUrlProviderFactory remoteWebUrlProviderFactory)
        {
            _log = log;
            _gvWriter = gvWriter;
            _lesserBranchSelectorFactory = lesserBranchSelectorFactory;
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

            ILesserBranchSelector lesserBranchSelector = _lesserBranchSelectorFactory.MakeSelector();
            lesserBranchSelector.PreProcessAllBranches(tree.Branches, options.LesserBranchesRegex);

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

        private void WriteCurrentBranchesLabels(
            IEnumerable<IBranch> currentBranches,
            IRemoteWebUrlProvider remoteUrlProvider)
        {
            _gvWriter.Comment("Branch names.");

            _gvWriter.SetNodeAttributes(
                AttrSet.Empty
                    .Shape(Shape.None)
                    .FixedSize(false)
                    .PenWidth(0)
                    .FillColor(GraphVizColor.None)
                    .Width(0)
                    .Height(0)
                    .Margin(0.05m)
            );

            foreach (IBranch b in currentBranches)
            {
                using (_gvWriter.StartSubGraph())
                {
                    _gvWriter.RawAttributes(AttrSet.Empty.Rank(RankType.Sink));

                    string url = remoteUrlProvider?.GetBranchLink(b);

                    _gvWriter.Node(
                        b,
                        AttrSet.Empty
                            .Label(b.Label)
                            .Group(b.Label)
                            .Url(url)
                    );
                }
            }
        }

        private void WriteFooter()
        {
            _gvWriter.EndGraph();
        }

        private void WriteHeader()
        {
            _gvWriter.StartGraph(GraphMode.Digraph, true);

            _gvWriter.SetGraphAttributes(
                AttrSet.Empty
                    .Rankdir(Rankdir.LR)
                    .NodeSep(0.2m)
                    .RankSep(0.1m)
                    .ForceLabels(false));

            _gvWriter.SetNodeAttributes(AttrSet.Empty.Style(Style.Filled));
        }

        private void WriteInBranchEdge(
            INode parentNode,
            INode childNote)
        {
            IAttrSet attrSet = AttrSet.Empty;
            bool drawDotted = childNote.AbsorbedParentCommits.Any();
            if (drawDotted)
            {
                attrSet.Style(Style.Dotted);
            }

            _gvWriter.Edge(parentNode, childNote, attrSet);
        }

        private void WriteNode(INode n, IRemoteWebUrlProvider remoteUrlProvider)
        {
            string url = remoteUrlProvider?.GetCommitLink(n.Commit);

            _gvWriter.Node(
                n,
                AttrSet.Empty
                    .Width(0.2m)
                    .Height(0.2m)
                    .Url(url)
            );
        }

        private void WriteNodes(
            ITree tree,
            IBranchingStrategy branchingStrategy,
            IBranch[] currentBranches,
            IRemoteWebUrlProvider remoteUrlProvider,
            PairList<INode, INode> otherLinks)
        {
            _gvWriter.Comment("Graph.");

            _gvWriter.SetNodeAttributes(
                AttrSet.Empty
                    .Width(0.2m)
                    .Height(0.2m)
                    .FixedSize(true)
                    .Label(string.Empty)
                    .Margin(0.11m, 0.055m)
                    .Shape(Shape.Circle)
                    .PenWidth(2)
                    .FillColor(Color.Red)
            );

            _gvWriter.Comment("Branches.");

            var firstNodeDates = new Dictionary<IBranch, DateTimeOffset?>();
            foreach (IBranch b in currentBranches)
            {
                DateTimeOffset? firstNodeDate = tree.EnumerateNodes(b).FirstOrDefault()?.Commit?.CommittedWhen;
                firstNodeDates.Add(b, firstNodeDate);
            }

            IBranch[] currentBranchesSorted =
                branchingStrategy.SortForWritingDescending(currentBranches, firstNodeDates).ToArray();

            foreach (IBranch b in currentBranchesSorted)
            {
                // Header.
                // 0 - branch short name
                // 1 - color
                string htmlString = branchingStrategy.GetHexColorFor(b);

                _gvWriter.SetNodeAttributes(
                    AttrSet.Empty
                        .Group(b.Label)
                        .FillColor(htmlString)
                        .Color(htmlString)
                );

                _gvWriter.SetEdgeAttributes(
                    AttrSet.Empty
                        .PenWidth(2)
                        .Color(htmlString)
                );

                using (_gvWriter.StartSubGraph())
                {
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
                        foreach (INode child in currentNode.Children)
                        {
                            otherLinks.Add(currentNode, child);
                        }
                    }

                    foreach (Tuple<INode, INode> tuple in edgesInBranch.EnumerateItems())
                    {
                        WriteInBranchEdge(tuple.Item1, tuple.Item2);
                    }
                }

                // Now link to the branch name node.
                // 0 - last node in branch
                // 1 - branch short name
                _gvWriter.Edge(
                    b.Tip,
                    b,
                    AttrSet.Empty
                        .Color("#b0b0b0")
                        .Style(Style.Dotted)
                        .ArrowHead(ArrowType.None)
                );
                _gvWriter.EmptyLine();
            }

            INode[] allLeftOvers = tree.Nodes.Where(n => tree.GetContainingBranch(n) == null).ToArray();
            if (allLeftOvers.Length > 0)
            {
                // Left-overs, without branches.
                _gvWriter.SetNodeAttributes(
                    AttrSet.Empty
                        .Group("")
                        .FillColor(Color.White)
                        .Color(Color.Black)
                );
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
            _gvWriter.Comment("All other edges.");
            _gvWriter.SetEdgeAttributes(
                AttrSet.Empty
                    .PenWidth(1)
                    .Color(Color.Black)
            );

            foreach (Tuple<INode, INode> pair in otherLinks.EnumerateItems())
            {
                INode nodeA = pair.Item1;
                INode nodeB = pair.Item2;
                string url = remoteUrlProvider?.GetCompareCommitsLink(nodeA.Commit, nodeB.Commit);

                _gvWriter.Edge(
                    nodeA,
                    nodeB,
                    AttrSet.Empty
                        .Url(url)
                );
            }

            _gvWriter.EmptyLine();
        }

        private void WriteTagsAndOrphanedBranches(
            ITag[] tags,
            IBranch[] orphanedBranches,
            IRemoteWebUrlProvider remoteUrlProvider)
        {
            _gvWriter.Comment("Orphaned branches.");

            foreach (IBranch b in orphanedBranches)
            {
                string url = remoteUrlProvider?.GetBranchLink(b);

                _gvWriter.Node(
                    b,
                    AttrSet.Empty
                        .Label(b.Label)
                        .Url(url)
                );
            }

            _gvWriter.Comment("Tags.");

            _gvWriter.SetNodeAttributes(
                AttrSet.Empty
                    .Shape(Shape.Cds)
                    .FixedSize(false)
                    .FillColor("#C6C6C6")
                    .PenWidth(1)
                    .Margin(0.11m, 0.055m)
            );

            foreach (ITag tag in tags)
            {
                string url = remoteUrlProvider?.GetTagLink(tag);

                _gvWriter.Node(
                    tag,
                    AttrSet.Empty
                        .Label(tag.Label)
                        .Url(url)
                );
            }
        }

        private void WriteTagsAndOrphanedBranchesConnections(
            IEnumerable<ITag> tags,
            IEnumerable<IBranch> orphanedBranches)
        {
            _gvWriter.Comment("Orphaned branches links.");

            _gvWriter.SetEdgeAttributes(
                AttrSet.Empty
                    .Color("#b0b0b0")
                    .Style(Style.Dotted)
                    .ArrowHead(ArrowType.None)
                    .Len(0.3m)
            );

            foreach (IBranch b in orphanedBranches)
            {
                _gvWriter.Edge(b.Tip, b, null);
            }

            _gvWriter.EmptyLine();

            _gvWriter.Comment("Tags.");

            _gvWriter.SetEdgeAttributes(
                AttrSet.Empty
                    .PenWidth(1)
            );

            foreach (ITag tag in tags)
            {
                using (_gvWriter.StartSubGraph())
                {
                    _gvWriter.RawAttributes(AttrSet.Empty.Rank(RankType.Same));

                    _gvWriter.Edge(tag.Tip, tag);
                }
            }
        }
    }
}