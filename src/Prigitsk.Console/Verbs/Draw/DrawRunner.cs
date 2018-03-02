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

        private string PrepareTargetPath()
        {
            string targetDirectory = Options.Target ?? Options.Repository;
            string targetName = Options.Output;
            if (string.IsNullOrWhiteSpace(targetName))
            {
                DirectoryInfoBase di = _fileSystem.DirectoryInfo.FromDirectoryName(Options.Repository);
                targetName = di.Name + "." + Options.Format;
            }

            _fileSystem.Directory.CreateDirectory(targetDirectory);

            string targetPath = _fileSystem.Path.Combine(targetDirectory, targetName);
            return targetPath;
        }

        protected override void RunInternal()
        {
            // Get the immutable repository information.
            IRepositoryData repositoryData = _loader.LoadFrom(Options.Repository);

            // Pick the remote to work on.
            IRemote remoteToUse = _remoteHelper.PickRemote(repositoryData, Options.RemoteToUse);

            // Create the tree.
            IBranchingStrategy strategy = _strategyProvider.GetStrategy();
            ITree tree = _treeBuilder.Build(repositoryData, remoteToUse, strategy, TreeBuildingOptions.Default);

            // Simplify the tree.
            SimplificationOptions soptions = SimplificationOptions.Default;
            soptions.AggressivelyRemoveFirstBranchNodes = false;

            _simplifier.Simplify(tree, soptions);

            string tempPath = _fileSystem.Path.GetTempFileName();
            tempPath = _fileSystem.Path.ChangeExtension(tempPath, "dot");

            using (ITextWriter textWriter = _fileWriterFactory.OpenForWriting(tempPath, Encoding.ASCII))
            {
                ITreeRenderer treeRenderer = _treeRendererFactory.CreateRenderer(textWriter);
                treeRenderer.Render(tree, remoteToUse, strategy, TreeRenderingOptions.Default);
            }

            string targetPath = PrepareTargetPath();

            string graphVizCommand = _appPathProvider.GetProperAppPath(ExternalApp.GraphViz);
            string graphVizArgs = $@"""{tempPath}"" -Tsvg -o""{targetPath}""";
            Log.Info($"Starting GraphViz with arguments: [{graphVizArgs}].");
            int code = _processRunner.Execute(graphVizCommand, graphVizArgs);
            if (code != 0)
            {
                Log.Error("GraphViz execution failed.");
            }
            else
            {
                Log.Info("GraphViz execution succeeded.");
            }
        }
    }
}