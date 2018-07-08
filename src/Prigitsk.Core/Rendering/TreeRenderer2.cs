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
using Prigitsk.Core.Strategy;
using Prigitsk.Framework;

namespace Prigitsk.Core.Rendering
{
    public sealed class TreeRenderer2 : ITreeRenderer
    {
        private readonly IGraphVizWriter _gvWriter;
        private readonly ILogger _log;
        private readonly IRemoteWebUrlProviderFactory _remoteWebUrlProviderFactory;

        public TreeRenderer2(
            ILogger<TreeRenderer> log,
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
            IBranchesKnowledge branchesKnowledge,
            ITreeRenderingOptions options)
        {
            IRemoteWebUrlProvider remoteUrlProvider =
                _remoteWebUrlProviderFactory.CreateUrlProvider(usedRemote.Url, options.ForceTreatAsGitHub);

            WriteHeader();



         
            WriteFooter();
        }

        private IEnumerable<IBranch> SortByFirstNodeDates(
            IEnumerable<IBranch> currentBranches,
            IDictionary<IBranch, DateTimeOffset?> firstNodeDates)
        {
            IComparer<IBranch> byDateComparer = new BranchSorterByDate(firstNodeDates);

            return currentBranches.OrderBy(b => b, byDateComparer);
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

            _gvWriter.SetNodeAttributes(AttrSet.Empty
                .FontName("Consolas")
                .FontSize(8)
                .Style(Style.Filled));
        }

    }
}