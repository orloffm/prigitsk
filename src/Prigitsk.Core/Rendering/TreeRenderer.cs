using System.Linq;
using Microsoft.Extensions.Logging;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Remotes;
using Prigitsk.Core.Tools;
using Prigitsk.Core.Tree;

namespace Prigitsk.Core.Rendering
{
    public sealed class TreeRenderer : ITreeRenderer
    {
        private readonly ILogger _log;
        private readonly IRemoteWebUrlProviderFactory _remoteWebUrlProviderFactory;

        public TreeRenderer(ILogger log, IRemoteWebUrlProviderFactory remoteWebUrlProviderFactory)
        {
            _log = log;
            _remoteWebUrlProviderFactory = remoteWebUrlProviderFactory;
        }

        public void Render(ITree tree, ITextWriter textWriter, IRemote usedRemote, ITreeRenderingOptions options)
        {
            IRemoteWebUrlProvider remoteUrlProvider = _remoteWebUrlProviderFactory.CreateUrlProvider(usedRemote, options.TreatRepositoryAsGitHub);

            WriteHeader(textWriter);

            WriteCurrentBranchesLabels(tree, textWriter, remoteUrlProvider);

            // Tags and orphaned branches names,
            ITag[] tags = tree.Tags.ToArray();

            OriginBranch[] orphanedBranches = graph.GetOrphanedBranches();

            WriteTagsAndOrphanedBranches(sb, tags, orphanedBranches);
        }

        private void WriteCurrentBranchesLabels(ITree tree, ITextWriter textWriter, IRemoteWebUrlProvider remoteUrlProvider)
        {
            textWriter.AppendLine(
                @"// branch names
    node[shape = none, fixedsize = false, penwidth = O, fillcolor = none, width = 0, height = 0, margin =""0.05""];");
            // 0 - branch short name
            // 1 - branch label
            // 2 - repository path
            const string currentBranchLabelFormat = @"subgraph {{
rank = sink;
    {0} [label=""{1}"", group=""{0}"", URL=""{2}""];
}}";
            foreach (IBranch b in tree.Branches)
            {
                string url = remoteUrlProvider?.GetBranchLink(b);

                string text = string.Format(
                    currentBranchLabelFormat,
                    MakeHandle(b),
                    b.Label,
                    url);
                textWriter.AppendLine(text);
            }
        }

        private string MakeHandle(IPointer pointerObject)
        {
            string pointerLabel = pointerObject.Label
                .Trim()
                .Replace(".", "_")
                .Replace("-", "_")
                .Replace(" ", "_");

            return  pointerLabel;
        }

        private void WriteHeader(ITextWriter textWriter)
        {
            textWriter.AppendLine(
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
    }
}