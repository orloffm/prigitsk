using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Logging;
using Prigitsk.Core;
using Prigitsk.Core.Graph;
using Prigitsk.Core.Graph.Making;
using Prigitsk.Core.Graph.Strategy;
using Prigitsk.Core.Graph.Writing;
using Prigitsk.Core.Nodes;
using Prigitsk.Core.Nodes.Loading;
using Prigitsk.Core.Tools;

namespace Prigitsk.Console.Verbs.Draw
{
    public class DrawRunner : VerbRunnerBase<IDrawRunnerOptions>, IDrawRunner
    {
        private const string DotPath = @"C:\apps\graphviz\dot.exe";

        public DrawRunner(IDrawRunnerOptions options, ILogger log)
            : base(options, log: log)
        {
        }

        protected override void RunInternal()
        {
            ExtractionOptions extractOptions = new ExtractionOptions
            {
                ExtractStats = false
            };
            string repositoryPath = FindRepositoryPath();
            string gitSubDirectory = Path.Combine(repositoryPath, path2: ".git");
            IProcessRunner processRunner = new ProcessRunner();
            INodeLoader loader = new NodeLoader(processRunner);
            loader.LoadFrom(gitSubDirectory, extractOptions: extractOptions);
            string writeTo = Path.Combine(repositoryPath, path2: "bin");
            Directory.CreateDirectory(writeTo);
            WriteToFileAndMakeSvg(loader, repositoryPath: writeTo, fileName: "full.dot", pickStrategy: PickAll);
            WriteToFileAndMakeSvg(
                loader,
                repositoryPath: writeTo,
                fileName: "no-tags.dot",
                pickStrategy: PickNoTags);
            WriteToFileAndMakeSvg(
                loader,
                repositoryPath: writeTo,
                fileName: "simple.dot",
                pickStrategy: PickSimplified);
            string runBat = Path.Combine(repositoryPath, path2: "run.bat");
            if (File.Exists(runBat))
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.WorkingDirectory = repositoryPath;
                psi.FileName = runBat;
                Process.Start(psi);
            }
        }

        internal static bool PickAll(Pointer b)
        {
            return true;
        }

        internal static bool PickNoTags(Pointer b)
        {
            if (b is Tag)
            {
                return false;
            }

            return PickAll(b);
        }

        internal static bool PickSimplified(Pointer b)
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

        private static string FindRepositoryPath()
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

        private static void WriteToFileAndMakeSvg(
            INodeLoader loader,
            string repositoryPath,
            string fileName,
            Func<Pointer, bool> pickStrategy)
        {
            WriteToDotFile(
                loader,
                repositoryPath: repositoryPath,
                fileName: fileName,
                pickStrategy: pickStrategy);
            ConvertTo(repositoryPath, fileName: fileName, format: "svg");
            ConvertTo(repositoryPath, fileName: fileName, format: "pdf");
        }

        private static void ConvertTo(string repositoryPath, string fileName, string format)
        {
            string svgFileName = Path.ChangeExtension(fileName, extension: format);
            string arguments = string.Format("{0} -T{2} -o{1}", fileName, arg1: svgFileName, arg2: format);
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.Arguments = arguments;
            psi.FileName = DotPath;
            psi.WorkingDirectory = repositoryPath;
            Process.Start(psi);
        }

        private static void WriteToDotFile(
            INodeLoader loader,
            string repositoryPath,
            string fileName,
            Func<Pointer, bool> pickStrategy)
        {
            Node[] allNodes = loader.GetNodesCollection();
            // What branches we have.
            IBranchingStrategy bs = new CommonFlowBranchingStrategy();
            // Try to distribute the nodes among the branches,
            // according to the branching strategy.
            IBranchAssumer ba = new BranchAssumer(bs, pickStrategy: pickStrategy);
            IAssumedGraph assumedGraph = ba.AssumeTheBranchGraph(allNodes);
            INodeCleaner cleaner = new NodeCleaner();
            SimplificationOptions options = new SimplificationOptions
            {
                PreventSimplification = false,
                AggressivelyRemoveFirstBranchNodes = true,
                LeaveNodesAfterLastMerge = false
            };
            cleaner.CleanUpGraph(assumedGraph, options: options);
            // Write the graph.
            // TODO: extract from git remote -v?
            ITreeWriter writer
                = new TreeWriter(@"https://github.com/torvalds/linux");
            string graphContent = writer.GenerateGraph(assumedGraph, branchingStrategy: bs);
            string dotpath = Path.Combine(repositoryPath, path2: fileName);
            File.WriteAllText(dotpath, contents: graphContent);
        }
    }
}