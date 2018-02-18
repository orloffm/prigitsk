using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Logging;
using Prigitsk.Console.General.Programs;
using Prigitsk.Core.Graph;
using Prigitsk.Core.Graph.Making;
using Prigitsk.Core.Graph.Strategy;
using Prigitsk.Core.Graph.Writing;
using Prigitsk.Core.Nodes;
using Prigitsk.Core.RepoData;

namespace Prigitsk.Console.Verbs.Draw
{
    public class DrawRunner : VerbRunnerBase<IDrawRunnerOptions>, IDrawRunner
    {
        private readonly IExternalAppPathProvider _appPathProvider;
        private readonly IRepositoryDataLoader _loader;

        public DrawRunner(
            IDrawRunnerOptions options,
            IRepositoryDataLoader loader,
            IExternalAppPathProvider appPathProvider,
            ILogger log)
            : base(options, log)
        {
            _loader = loader;
            _appPathProvider = appPathProvider;
        }

        protected override void RunInternal()
        {
            string repositoryPath = FindRepositoryPath();
            string gitSubDirectory = Path.Combine(repositoryPath, ".git");
            //IProcessRunner processRunner = new ProcessRunner();
            IRepositoryData repositoryData = _loader.LoadFrom(gitSubDirectory);

            string writeTo = Path.Combine(repositoryPath, "bin");
            Directory.CreateDirectory(writeTo);
            WriteToFileAndMakeSvg(repositoryData, writeTo, "full.dot", PickAll);
            //WriteToFileAndMakeSvg(
            //    repositoryData,
            //    writeTo,
            //    "no-tags.dot",
            //    PickNoTags);
            //WriteToFileAndMakeSvg(
            //    repositoryData,
            //    writeTo,
            //    "simple.dot",
            //    PickSimplified);
        }

        internal bool PickAll(Pointer b)
        {
            return true;
        }

        internal bool PickNoTags(Pointer b)
        {
            if (b is Tag)
            {
                return false;
            }

            return PickAll(b);
        }

        internal bool PickSimplified(Pointer b)
        {
            string label = b.Label.ToLower();
            if (b is Tag)
            {
                //if (label.StartsWith("6"))
                //{
                //    return true;
                //}
            }
            else
            {
                if (label == "develop"
                    || label.Contains("master")
                    || label.Contains("release")
                    || label.EndsWith("-rc")
                    // || label.Contains("mifid")
                )
                {
                    return true;
                }
            }

            return false;
        }

        private string FindRepositoryPath()
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (true)
            {
                if (di == null)
                {
                    throw new IOException();
                }

                if (di.GetDirectories(".git", SearchOption.TopDirectoryOnly).Length == 1)
                {
                    return di.FullName;
                }

                di = di.Parent;
            }
        }

        private void WriteToFileAndMakeSvg(
            IRepositoryData repositoryData,
            string directoryToWriteTo,
            string fileName,
            Func<Pointer, bool> pickStrategy)
        {
            WriteToDotFile(repositoryData, directoryToWriteTo, fileName, pickStrategy);

        //    ConvertTo(directoryToWriteTo, fileName, "svg");
        //    ConvertTo(directoryToWriteTo, fileName, "pdf");
        //}

        //private void ConvertTo(string repositoryPath, string fileName, string format)
        //{
        //    string svgFileName = Path.ChangeExtension(fileName, format);
        //    string arguments = string.Format("{0} -T{2} -o{1}", fileName, svgFileName, format);
        //    ProcessStartInfo psi = new ProcessStartInfo();
        //    psi.Arguments = arguments;
        //    psi.FileName = _appPathProvider.GetProperAppPath(ExternalApp.GraphViz);
        //    psi.WorkingDirectory = repositoryPath;
        //    Process.Start(psi);
        }

        private void WriteToDotFile(
            IRepositoryData repositoryData,
            string repositoryPath,
            string fileName,
            Func<Pointer, bool> pickStrategy)
        {
            // What branches we have.
            IBranchingStrategy bs = new CommonFlowBranchingStrategy();
            // Try to distribute the nodes among the branches,
            // according to the branching strategy.
            IBranchAssumer ba = new BranchAssumer(bs, new TreeWalker(), pickStrategy);
            IAssumedGraph assumedGraph = ba.AssumeTheBranchGraph(repositoryData);
            INodeCleaner cleaner = new NodeCleaner(new TreeManipulator(), new TreeWalker());
            SimplificationOptions options = new SimplificationOptions
            {
                PreventSimplification = false,
                AggressivelyRemoveFirstBranchNodes = true,
                LeaveNodesAfterLastMerge = false
            };
            cleaner.CleanUpGraph(assumedGraph, options);
            // Write the graph.
            // TODO: extract from git remote -v?
            ITreeWriter writer
                = new TreeWriter(@"https://github.com/torvalds/linux");
            string graphContent = writer.GenerateGraph(assumedGraph, bs);
            string dotpath = Path.Combine(repositoryPath, fileName);
            File.WriteAllText(dotpath, graphContent);
        }
    }
}