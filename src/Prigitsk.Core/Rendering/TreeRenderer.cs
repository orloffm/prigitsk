using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.Extensions.Logging;
using OrlovMikhail.GraphViz.Writing;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Entities.Comparers;
using Prigitsk.Core.Graph;
using Prigitsk.Core.Remotes;
using Prigitsk.Core.Rendering.Styling;
using Prigitsk.Core.Strategy;
using Prigitsk.Framework;

namespace Prigitsk.Core.Rendering
{
    public sealed class TreeRenderer : ITreeRenderer
    {
        private readonly IGraphVizWriter _gvWriter;
        private readonly ILogger _log;
        private readonly IRemoteWebUrlProviderFactory _remoteWebUrlProviderFactory;
        private readonly IStyleProvider _style;
        private readonly IGraphTooltipHelper _tooltipHelper;

        public TreeRenderer(
            ILogger<TreeRenderer> log,
            IGraphVizWriter gvWriter,
            IStyleProvider style,
            IGraphTooltipHelper tooltipHelper,
            IRemoteWebUrlProviderFactory remoteWebUrlProviderFactory)
        {
            _log = log;
            _gvWriter = gvWriter;
            _style = style;
            _tooltipHelper = tooltipHelper;
            _remoteWebUrlProviderFactory = remoteWebUrlProviderFactory;
        }

        public void Render(
            ITree tree,
            IRemote usedRemote,
            IBranchesKnowledge branchesKnowledge,
            ITreeRenderingOptions options)
        {
            IRemoteWebUrlProvider remoteUrlProvider =
                _remoteWebUrlProviderFactory.CreateUrlProvider(usedRemote.Url, options.ForceTreatAsGitHub);

            WriteHeader();

            IBranch[] branchesWithNodes = tree.Branches.Where(b => tree.EnumerateNodes(b).Any()).ToArray();
            IBranch[] currentBranchesSorted = OrderByFirstCommitDate(branchesWithNodes, tree).ToArray();

            IPairList<INode, INode> unwrittenMerges = new PairList<INode, INode>();
            IPairList<INode, INode> writtenMerges = new PairList<INode, INode>();

            // Main branches.
            foreach (IBranch b in currentBranchesSorted)
            {
                _gvWriter.EmptyLine();
                _gvWriter.Comment($"Branch {b.Label}.");

                using (_gvWriter.StartSubGraph())
                {
                    _gvWriter.SetNodeAttributes(AttrSet.Empty.Group(b.Label));

                    Color drawColor = branchesKnowledge.GetSuggestedDrawingColorFor(b);
                    bool isLesser = branchesKnowledge.IsAWorkItemBranch(b);

                    IAttrSet nodeStyle = _style.GetBranchNodeStyle(drawColor, isLesser);
                    IAttrSet edgeStyle = _style.GetBranchEdgeStyle(drawColor, isLesser);

                    _gvWriter.SetNodeAttributes(nodeStyle);
                    _gvWriter.SetEdgeAttributes(edgeStyle);

                    INode[] nodes = tree.EnumerateNodes(b).ToArray();

                    for (int i = 0; i < nodes.Length; i++)
                    {
                        INode currentNode = nodes[i];

                        WriteNode(currentNode, remoteUrlProvider);

                        if (i == 0)
                        {
                            // Starting node.
                            INode parent = currentNode.Parents.FirstOrDefault();
                            bool hasParent = parent != null;
                            if (!hasParent)
                            {
                                _gvWriter.Comment("Starting line.");
                                string id = string.Format($"{currentNode.Treeish}_start");
                                // Write starting empty node.
                                using (_gvWriter.StartSubGraph())
                                {
                                    _gvWriter.RawAttributes(AttrSet.Empty.Rank(RankType.Source));

                                    _gvWriter.Node(id, AttrSet.Empty.Width(0).Height(0).PenWidth(0));
                                }

                                _gvWriter.Edge(id, currentNode, _style.EdgeBranchStartVirtual);
                                _gvWriter.EmptyLine();
                            }
                            else
                            {
                                // It is some other branch, and we draw that edge.
                                WriteEdge(parent, currentNode, remoteUrlProvider);
                                writtenMerges.Add(parent, currentNode);
                            }
                        }

                        bool isLast = i == nodes.Length - 1;
                        INode nextNode = isLast ? null : nodes[i + 1];

                        if (!isLast)
                        {
                            // Edge to the next node.
                            WriteEdge(currentNode, nextNode, remoteUrlProvider);
                        }
                        else
                        {
                            WriteBranchPointer(b, tree, isLesser, remoteUrlProvider);
                        }

                        INode[] otherChildren = currentNode.Children.Except(nextNode).ToArray();
                        foreach (INode child in otherChildren)
                        {
                            unwrittenMerges.Add(currentNode, child);
                        }
                    }
                }
            }

            IBranch[] branchesWithoutNodes = tree.Branches.Except(branchesWithNodes).ToArray();
            foreach (IBranch b in branchesWithoutNodes)
            {
                bool isLesser = branchesKnowledge.IsAWorkItemBranch(b);
                WriteBranchPointer(b, tree, isLesser, remoteUrlProvider);
            }

            // Tags.
            ITag[] tags = tree.Tags.ToArray();
            foreach (ITag t in tags)
            {
                _gvWriter.EmptyLine();

                INode n = tree.GetNode(t.Tip);
                string id = MakeNodeIdForPointerLabel(n, t);

                using (_gvWriter.StartSubGraph())
                {
                    _gvWriter.Comment($"Tag {t.Label}.");
                    _gvWriter.RawAttributes(AttrSet.Empty.Rank(RankType.Same));

                    string url = remoteUrlProvider?.GetTagLink(t);

                    _gvWriter.Node(id, _style.LabelTag.Label(t.Label).Url(url));

                    _gvWriter.Edge(n, id, _style.EdgeTagLabel);
                }
            }

            INode[] allLeftOvers = tree.Nodes.Where(n => tree.GetContainingBranch(n) == null).ToArray();
            if (allLeftOvers.Length > 0)
            {
                using (_gvWriter.StartSubGraph())
                {
                    _gvWriter.EmptyLine();
                    _gvWriter.Comment("Leftover nodes.");
                    _gvWriter.SetNodeAttributes(_style.NodeOrphaned);

                    foreach (INode currentNode in allLeftOvers)
                    {
                        WriteNode(currentNode, remoteUrlProvider);
                        // Remember children.
                        foreach (INode child in currentNode.Children)
                        {
                            unwrittenMerges.Add(currentNode, child);
                        }
                    }
                }
            }

            PairList<INode, INode> edgesToWrite = unwrittenMerges.Except(writtenMerges).ToPairList();
            if (edgesToWrite.Count > 0)
            {
                using (_gvWriter.StartSubGraph())
                {
                    _gvWriter.EmptyLine();
                    _gvWriter.Comment("Other edges.");
                    _gvWriter.SetEdgeAttributes(_style.EdgeOther);

                    foreach (Tuple<INode, INode> edge in edgesToWrite)
                    {
                        _gvWriter.Edge(edge.Item1, edge.Item2);
                    }
                }
            }

            WriteFooter();
        }

        private static string MakeNodeIdForPointerLabel(INode node, IPointer pointer)
        {
            string id = string.Format($"{node.Treeish}_{pointer.Label}");
            return id;
        }

        private IEnumerable<IBranch> OrderByFirstCommitDate(IEnumerable<IBranch> currentBranches, ITree tree)
        {
            var brancheList = new List<IBranch>();
            var firstNodeDates = new Dictionary<IBranch, DateTimeOffset?>();
            foreach (IBranch b in currentBranches)
            {
                brancheList.Add(b);
                DateTimeOffset? firstNodeDate = tree.EnumerateNodes(b).FirstOrDefault()?.Commit?.Committer?.When;
                firstNodeDates.Add(b, firstNodeDate);
            }

            IComparer<IBranch> byDateComparer = new BranchSorterByDate(firstNodeDates);
            brancheList.Sort(byDateComparer);
            return brancheList;
        }

        private void WriteBranchLabel(string id, IBranch b, IRemoteWebUrlProvider remoteUrlProvider)
        {
            IAttrSet style = _style.LabelBranch;
            string url = remoteUrlProvider?.GetBranchLink(b);

            _gvWriter.Node(id, style.Label(b.Label).Url(url));
        }

        private void WriteBranchPointer(IBranch b, ITree tree, bool isLesser, IRemoteWebUrlProvider remoteUrlProvider)
        {
            INode pointedNode = tree.GetNode(b.Tip);
            bool isOwned = tree.GetContainingBranch(pointedNode) == b;

            // Ending textual node.
            string branchLabelId = MakeNodeIdForPointerLabel(pointedNode, b);

            bool shouldSink = !isLesser && isOwned;
            using (_gvWriter.StartSubGraph())
            {
                if (shouldSink)
                {
                    _gvWriter.RawAttributes(AttrSet.Empty.Rank(RankType.Sink));
                }

                WriteBranchLabel(branchLabelId, b, remoteUrlProvider);
            }

            WriteBranchEndingEdge(pointedNode, branchLabelId, b, remoteUrlProvider);
        }

        private void WriteBranchEndingEdge(
            INode pointedNode,
            string id,
            IBranch b,
            IRemoteWebUrlProvider remoteUrlProvider
        )
        {
            IAttrSet style = _style.EdgeBranchLabel;
            string url = remoteUrlProvider?.GetBranchLink(b);

            _gvWriter.Edge(pointedNode, id, style.Url(url));
        }

        private void WriteEdge(INode a, INode b, IRemoteWebUrlProvider remoteUrlProvider)
        {
            string url = remoteUrlProvider?.GetCompareCommitsLink(a.Commit, b.Commit);
            string tooltip = _tooltipHelper.MakeEdgeTooltip(a, b);

            IAttrSet attrSet = AttrSet.Empty
                .Url(url)
                .Tooltip(tooltip);

            bool hasMergedCommits = b.AbsorbedParentCommits.Any();
            if (hasMergedCommits)
            {
                attrSet = attrSet.Add(_style.EdgeMergedCommits);
            }

            _gvWriter.Edge(a, b, attrSet);
        }

        private void WriteFooter()
        {
            _gvWriter.EndGraph();
        }

        private void WriteHeader()
        {
            _gvWriter.StartGraph(GraphMode.Digraph, true);

            IAttrSet graphStyle = _style.GraphGeneric;

            _gvWriter.SetGraphAttributes(graphStyle);

            IAttrSet genericNodeStyle = _style.NodeGeneric;

            _gvWriter.SetNodeAttributes(genericNodeStyle);

            IAttrSet genericEdgeStyle = _style.EdgeGeneric;

            _gvWriter.SetEdgeAttributes(genericEdgeStyle);
        }

        private void WriteNode(INode n, IRemoteWebUrlProvider remoteUrlProvider)
        {
            string url = remoteUrlProvider?.GetCommitLink(n.Commit);
            string tooltip = _tooltipHelper.MakeNodeTooltip(n);

            IAttrSet nodeAttrs = AttrSet.Empty
                .Url(url)
                .Tooltip(tooltip);

            _gvWriter.Node(n, nodeAttrs);
        }
    }
}