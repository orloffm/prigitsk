﻿using System;
using System.Diagnostics;
using System.IO;
using Autofac;
using GitWriter.Abstractions;
using GitWriter.Core;
using GitWriter.Core.Graph;
using GitWriter.Core.Graph.Making;
using GitWriter.Core.Graph.Strategy;
using GitWriter.Core.Graph.Writing;
using GitWriter.Core.Nodes;
using GitWriter.Core.Nodes.Loading;
using GitWriter.Windows;
using log4net.Config;

namespace ConGitWriter
{
    internal class Program
    {
        private const string DotPath = @"C:\apps\graphviz\dot.exe";

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

        private static void Main(string[] args)
        {
            ConfigureLog4Net();

            ContainerBuilder builder = new ContainerBuilder();
            IContainer container = builder.Build();

            ExtractionOptions extractOptions = new ExtractionOptions
            {
                ExtractStats = false
            };
            string repositoryPath = FindRepositoryPath();
            string gitSubDirectory = Path.Combine(repositoryPath, ".git");
            IProcessRunner processRunner = new ProcessRunner();
            INodeLoader loader = new NodeLoader(processRunner);
            loader.LoadFrom(gitSubDirectory, extractOptions);
            string writeTo = Path.Combine(repositoryPath, "bin");
            Directory.CreateDirectory(writeTo);
            WriteToFileAndMakeSvg(loader, writeTo, "full.dot", PickAll);
            WriteToFileAndMakeSvg(loader, writeTo, "no-tags.dot", PickNoTags);
            WriteToFileAndMakeSvg(loader, writeTo, "simple.dot", PickSimplified);
            string runBat = Path.Combine(repositoryPath, "run.bat");
            if (File.Exists(runBat))
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.WorkingDirectory = repositoryPath;
                psi.FileName = runBat;
                Process.Start(psi);
            }
        }

        private static void WriteToFileAndMakeSvg(
            INodeLoader loader,
            string repositoryPath,
            string fileName,
            Func<Pointer, bool> pickStrategy)
        {
            WriteToDotFile(loader, repositoryPath, fileName, pickStrategy);
            ConvertTo(repositoryPath, fileName, "svg");
            ConvertTo(repositoryPath, fileName, "pdf");
        }

        private static void ConvertTo(string repositoryPath, string fileName, string format)
        {
            string svgFileName = Path.ChangeExtension(fileName, format);
            string arguments = string.Format("{0} -T{2} -o{1}", fileName, svgFileName, format);
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
            IBranchAssumer ba = new BranchAssumer(bs, pickStrategy);
            IAssumedGraph assumedGraph = ba.AssumeTheBranchGraph(allNodes);
            INodeCleaner cleaner = new NodeCleaner();
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

        private static void ConfigureLog4Net()
        {
            XmlConfigurator.Configure();
        }
    }
}