using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using OrlovMikhail.GraphViz.Writing;
using Prigitsk.Console.General;
using Prigitsk.Console.General.Programs;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Graph;
using Prigitsk.Core.Remotes;
using Prigitsk.Core.Rendering;
using Prigitsk.Core.RepoData;
using Prigitsk.Core.Simplification;
using Prigitsk.Core.Strategy;
using Prigitsk.Framework;
using Prigitsk.Framework.IO;
using Thinktecture.IO;

namespace Prigitsk.Console.Verbs.Draw
{
    public class DrawRunner : VerbRunnerBase<IDrawRunnerOptions>, IDrawRunner
    {
        private readonly IExternalAppPathProvider _appPathProvider;
        private readonly IFileSystem _fileSystem;
        private readonly IGraphVizWriterFactory _graphVizFactory;
        private readonly IRepositoryDataLoader _loader;
        private readonly IProcessRunner _processRunner;
        private readonly IRemoteHelper _remoteHelper;
        private readonly ISimplifier _simplifier;
        private readonly IBranchingStrategyProvider _strategyProvider;
        private readonly ITextWriterFactory _textWriterFactory;
        private readonly ITreeBuilder _treeBuilder;
        private readonly ITreeRendererFactory _treeRendererFactory;
        private string _outputFormat;

        private IDirectoryInfo _repositoryDir;

        public DrawRunner(
            IDrawRunnerOptions options,
            IProcessRunner processRunner,
            IRepositoryDataLoader loader,
            ITreeBuilder treeBuilder,
            ISimplifier simplifier,
            IFileSystem fileSystem,
            ITreeRendererFactory treeRendererFactory,
            IGraphVizWriterFactory graphVizFactory,
            IRemoteHelper remoteHelper,
            ITextWriterFactory textWriterFactory,
            IExternalAppPathProvider appPathProvider,
            IBranchingStrategyProvider strategyProvider,
            ILogger<DrawRunner> log)
            : base(options, log)
        {
            _processRunner = processRunner;
            _loader = loader;
            _treeBuilder = treeBuilder;
            _simplifier = simplifier;
            _fileSystem = fileSystem;
            _treeRendererFactory = treeRendererFactory;
            _graphVizFactory = graphVizFactory;
            _remoteHelper = remoteHelper;
            _textWriterFactory = textWriterFactory;
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
            tempPath = _fileSystem.Path.ChangeExtension(tempPath, "gv");

            // Rendering options.
            TreeRenderingOptions renderingOptions = TreeRenderingOptions.Default;
            renderingOptions.ForceTreatAsGitHub = Options.ForceTreatAsGitHub;

            // ReSharper disable once AssignNullToNotNullAttribute
            using (IFileStream fileStream = _fileSystem.File.OpenWrite(tempPath))
            {
                using (IStreamWriter textWriter = _textWriterFactory.CreateStreamWriter(fileStream))
                {
                    IGraphVizWriter graphVizWriter = _graphVizFactory.CreateGraphVizWriter(textWriter);

                    ITreeRenderer treeRenderer = _treeRendererFactory.CreateRenderer(graphVizWriter);
                    treeRenderer.Render(tree, remoteToUse, strategy, renderingOptions);
                }
            }

            string targetPath = PrepareTargetPath();

            string graphVizCommand = _appPathProvider.GetProperAppPath(ExternalApp.GraphViz);
            string graphVizArgs = $@"""{tempPath}"" -T{_outputFormat} -o""{targetPath}""";
            Log.Debug($"Starting GraphViz with arguments: [{graphVizArgs}].");
            int code = _processRunner.Execute(graphVizCommand, graphVizArgs);
            if (code != 0)
            {
                Log.Fatal("GraphViz execution failed.");
            }
            else
            {
                Log.Info("GraphViz execution succeeded.");
                Log.Info($"Saved to {targetPath}.");
            }
        }

        private IDirectoryInfo GetRepositoryToUse()
        {
            string directoryToSearchFrom = null;

            if (!string.IsNullOrWhiteSpace(Options.Repository))
            {
                directoryToSearchFrom = Options.Repository;

                if (!_fileSystem.Directory.Exists(directoryToSearchFrom))
                {
                    string message =
                        $"The specified repository directory {directoryToSearchFrom} does not exist.";
                    Log.Fatal(message);
                    throw new LoggedAsFatalException(message);
                }
            }
            else
            {
                directoryToSearchFrom = _fileSystem.Directory.GetCurrentDirectory();
                Log.Info($"Will look for a Git repository in current directory '{directoryToSearchFrom}'.");
            }

            IDirectoryInfo di = TryFindGitRepositoryDirectory(directoryToSearchFrom);
            if (di == null)
            {
                string message =
                    "No Git directory found.";
                Log.Fatal(message);
                throw new LoggedAsFatalException(message);
            }

            Log.Debug("Using Git repository from {di.FullName}.");

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

        /// <summary>
        ///     Searches for a directory containing a ".git" directory
        ///     up from the current one.
        /// </summary>
        private IDirectoryInfo TryFindGitRepositoryDirectory(string source)
        {
            IDirectoryInfo di = _fileSystem.DirectoryInfo.Create(source);

            do
            {
                bool containsGitFolder = di.GetDirectories(".git", SearchOption.TopDirectoryOnly).Any();
                if (containsGitFolder)
                {
                    break;
                }

                di = di.Parent;
            } while (di != null);

            return di;
        }
    }
}