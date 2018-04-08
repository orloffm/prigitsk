using System;
using System.IO.Abstractions;
using System.Text;
using Microsoft.Extensions.Logging;
using Prigitsk.Console.Abstractions.TextWriter;
using Prigitsk.Console.General;
using Prigitsk.Console.General.Programs;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Graph;
using Prigitsk.Core.Remotes;
using Prigitsk.Core.Rendering;
using Prigitsk.Core.RepoData;
using Prigitsk.Core.Simplification;
using Prigitsk.Core.Strategy;
using Prigitsk.Core.Tools;

namespace Prigitsk.Console.Verbs.Draw
{
    public class DrawRunner : VerbRunnerBase<IDrawRunnerOptions>, IDrawRunner
    {
        private readonly IExternalAppPathProvider _appPathProvider;
        private readonly IFileSystem _fileSystem;
        private readonly IFileTextWriterFactory _fileWriterFactory;
        private readonly IRepositoryDataLoader _loader;
        private readonly IProcessRunner _processRunner;
        private readonly IRemoteHelper _remoteHelper;
        private readonly ISimplifier _simplifier;
        private readonly IBranchingStrategyProvider _strategyProvider;
        private readonly ITreeBuilder _treeBuilder;
        private readonly ITreeRendererFactory _treeRendererFactory;
        private string _outputFormat;

        private DirectoryInfoBase _repositoryDir;

        public DrawRunner(
            IDrawRunnerOptions options,
            IProcessRunner processRunner,
            IRepositoryDataLoader loader,
            ITreeBuilder treeBuilder,
            ISimplifier simplifier,
            IFileSystem fileSystem,
            ITreeRendererFactory treeRendererFactory,
            IRemoteHelper remoteHelper,
            IFileTextWriterFactory fileWriterFactory,
            IExternalAppPathProvider appPathProvider,
            IBranchingStrategyProvider strategyProvider,
            ILogger log)
            : base(options, log)
        {
            _processRunner = processRunner;
            _loader = loader;
            _treeBuilder = treeBuilder;
            _simplifier = simplifier;
            _fileSystem = fileSystem;
            _treeRendererFactory = treeRendererFactory;
            _remoteHelper = remoteHelper;
            _fileWriterFactory = fileWriterFactory;
            _appPathProvider = appPathProvider;
            _strategyProvider = strategyProvider;
        }

        protected override void Initialise()
        {
            _repositoryDir = GetRepositoryToUse();
            _outputFormat = Options.Format;

            if (string.IsNullOrWhiteSpace(_outputFormat))
            {
                Log.Info("Output format not specified. Will use SVG.");
                _outputFormat = "svg";
            }
        }

        protected override void RunInternal()
        {
            // Get the immutable repository information.

            IRepositoryData repositoryData = _loader.LoadFrom(_repositoryDir.FullName);

            // Pick the remote to work on.
            IRemote remoteToUse = _remoteHelper.PickRemote(repositoryData, Options.RemoteToUse);

            // Create the tree.
            IBranchingStrategy strategy = _strategyProvider.GetStrategy();
            ITree tree = _treeBuilder.Build(repositoryData, remoteToUse, strategy, TreeBuildingOptions.Default);

            SimplifyTree(tree);

            string tempPath = _fileSystem.Path.GetTempFileName();
            tempPath = _fileSystem.Path.ChangeExtension(tempPath, "dot");

            // Rendering options.
            TreeRenderingOptions renderingOptions = TreeRenderingOptions.Default;
            renderingOptions.ForceTreatAsGitHub = Options.ForceTreatAsGitHub;

            using (ITextWriter textWriter = _fileWriterFactory.OpenForWriting(tempPath, Encoding.ASCII))
            {
                ITreeRenderer treeRenderer = _treeRendererFactory.CreateRenderer(textWriter);
                treeRenderer.Render(tree, remoteToUse, strategy, renderingOptions);
            }

            string targetPath = PrepareTargetPath();

            string graphVizCommand = _appPathProvider.GetProperAppPath(ExternalApp.GraphViz);
            string graphVizArgs = $@"""{tempPath}"" -T{_outputFormat} -o""{targetPath}""";
            Log.Info($"Starting GraphViz with arguments: [{graphVizArgs}].");
            int code = _processRunner.Execute(graphVizCommand, graphVizArgs);
            if (code != 0)
            {
                Log.Error("GraphViz execution failed.");
            }
            else
            {
                Log.Info("GraphViz execution succeeded.");
                Log.Info($"Saved to {targetPath}.");
            }
        }

        private DirectoryInfoBase GetRepositoryToUse()
        {
            bool usingCurrentDir = false;
            string repositoryDirectory = Options.Repository;
            if (string.IsNullOrWhiteSpace(repositoryDirectory))
            {
                repositoryDirectory = _fileSystem.Directory.GetCurrentDirectory();
                Log.Info($"Using current directory '{repositoryDirectory}' as repository path.");
                usingCurrentDir = true;
            }

            DirectoryInfoBase di = _fileSystem.DirectoryInfo.FromDirectoryName(repositoryDirectory);
            if (!usingCurrentDir && !di.Exists)
            {
                throw new Exception($"The specified repository directory {repositoryDirectory} does not exist.");
            }

            return di;
        }

        private string PrepareTargetPath()
        {
            string targetDirectory = Options.TargetDirectory;
            if (string.IsNullOrWhiteSpace(targetDirectory))
            {
                targetDirectory = _repositoryDir.FullName;
                Log.Info($"Target directory not specified, will use repository directory.");
            }

            string targetFileName = Options.OutputFileName;
            if (string.IsNullOrWhiteSpace(targetFileName))
            {
                targetFileName = _repositoryDir.Name + "." + _outputFormat;
                Log.Info($"Target file name not specified, will use {targetFileName} instead.");
            }

            _fileSystem.Directory.CreateDirectory(targetDirectory);

            string targetPath = _fileSystem.Path.Combine(targetDirectory, targetFileName);
            Log.Debug($"Will save to {targetPath}.");
            return targetPath;
        }

        private void SimplifyTree(ITree tree)
        {
            bool simplify = !Options.PreventSimplification;
            if (simplify)
            {
                // Simplify the tree.
                SimplificationOptions simplificationOptions = SimplificationOptions.Default;
                simplificationOptions.LeaveTails = Options.LeaveHeads;
                simplificationOptions.FirstBranchNodeMayBeRemoved = Options.RemoveTails;
                simplificationOptions.KeepAllOrphans = Options.KeepAllOrphans;
                simplificationOptions.KeepOrphansWithTags = Options.KeepOrphansWithTags;

                _simplifier.Simplify(tree, simplificationOptions);
            }
        }
    }
}