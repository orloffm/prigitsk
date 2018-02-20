using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Prigitsk.Console.Abstractions.TextWriter;
using Prigitsk.Console.General.Programs;
using Prigitsk.Core.Rendering;
using Prigitsk.Core.RepoData;
using Prigitsk.Core.Simplification;
using Prigitsk.Core.Strategy;
using Prigitsk.Core.Tools;
using Prigitsk.Core.Tree;

namespace Prigitsk.Console.Verbs.Draw
{
    public class DrawRunner : VerbRunnerBase<IDrawRunnerOptions>, IDrawRunner
    {
        private readonly IExternalAppPathProvider _appPathProvider;
        private readonly IFileSystem _fileSystem;
        private readonly IFileTextWriterFactory _fileWriterFactory;
        private readonly IRepositoryDataLoader _loader;
        private readonly ISimplifier _simplifier;
        private readonly IBranchingStrategyProvider _strategyProvider;
        private readonly ITreeBuilder _treeBuilder;
        private readonly ITreeRenderer _treeRenderer;

        public DrawRunner(
            IDrawRunnerOptions options,
            IRepositoryDataLoader loader,
            ITreeBuilder treeBuilder,
            ISimplifier simplifier,
            IFileSystem fileSystem,
            ITreeRenderer treeRenderer,
            IFileTextWriterFactory fileWriterFactory,
            IExternalAppPathProvider appPathProvider,
            IBranchingStrategyProvider strategyProvider,
            ILogger log)
            : base(options, log)
        {
            _loader = loader;
            _treeBuilder = treeBuilder;
            _simplifier = simplifier;
            _fileSystem = fileSystem;
            _treeRenderer = treeRenderer;
            _fileWriterFactory = fileWriterFactory;
            _appPathProvider = appPathProvider;
            _strategyProvider = strategyProvider;
        }

        private string PrepareTargetPath()
        {
            string targetDirectory = Options.Target ?? Options.Repository;

            _fileSystem.Directory.CreateDirectory(targetDirectory);

            string targetPath = _fileSystem.Path.Combine(targetDirectory, Options.Output);
            return targetPath;
        }

        //internal bool PickAll(Pointer b)
        //{
        //    return true;
        //}

        //internal bool PickNoTags(Pointer b)
        //{
        //    if (b is Tag)
        //    {
        //        return false;
        //    }

        //    return PickAll(b);
        //}

        //internal bool PickSimplified(Pointer b)
        //{
        //    string label = b.Label.ToLower();
        //    if (b is Tag)
        //    {
        //        //if (label.StartsWith("6"))
        //        //{
        //        //    return true;
        //        //}
        //    }
        //    else
        //    {
        //        if (label == "develop"
        //            || label.Contains("master")
        //            || label.Contains("release")
        //            || label.EndsWith("-rc")
        //            // || label.Contains("mifid")
        //        )
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        protected override void RunInternal()
        {
            // Get the immutable repository information.
            IRepositoryData repositoryData = _loader.LoadFrom(Options.Repository);

            // Create the tree.
            IBranchingStrategy strategy = _strategyProvider.GetStrategy();
            ITree tree = _treeBuilder.Build(repositoryData, strategy, TreeBuildingOptions.Default);

            // Simplify the tree.
            _simplifier.Simplify(tree, SimplificationOptions.None);

            string targetPath = PrepareTargetPath();

            using (ITextWriter textWriter = _fileWriterFactory.OpenForWriting(targetPath))
            {
                _treeRenderer.Render(tree, textWriter, TreeRenderingOptions.Default);
            }

            // WriteToFileAndMakeSvg(repositoryData, targetPath, "full.dot", PickAll);
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

        //private void WriteToDotFile(
        //    IRepositoryData repositoryData,
        //    string repositoryPath,
        //    string fileName,
        //    Func<Pointer, bool> pickStrategy)
        //{
        //    // What branches we have.
        //    IBranchingStrategy bs = new CommonFlowBranchingStrategy();
        //    // Try to distribute the nodes among the branches,
        //    // according to the branching strategy.
        //    IBranchAssumer ba = new BranchAssumer(bs, new TreeWalker(), pickStrategy);
        //    IAssumedGraph assumedGraph = ba.AssumeTheBranchGraph(repositoryData);
        //    INodeCleaner cleaner = new NodeCleaner(new TreeManipulator(), new TreeWalker());
        //    SimplificationOptions options = new SimplificationOptions
        //    {
        //        PreventSimplification = false,
        //        AggressivelyRemoveFirstBranchNodes = true,
        //        LeaveNodesAfterLastMerge = false
        //    };
        //    cleaner.CleanUpGraph(assumedGraph, options);
        //    // Render the graph.
        //    // TODO: extract from git remote -v?
        //    ITreeRenderer writer
        //        = new TreeWriter(@"https://github.com/torvalds/linux");
        //    string graphContent = writer.GenerateGraph(assumedGraph, bs);
        //    string dotpath = Path.Combine(repositoryPath, fileName);
        //    File.WriteAllText(dotpath, graphContent);
        //}

        //private void WriteToFileAndMakeSvg(
        //    IRepositoryData repositoryData,
        //    string directoryToWriteTo,
        //    string fileName,
        //    Func<Pointer, bool> pickStrategy)
        //{
        //    WriteToDotFile(repositoryData, directoryToWriteTo, fileName, pickStrategy);

        //    //    ConvertTo(directoryToWriteTo, fileName, "svg");
        //    //    ConvertTo(directoryToWriteTo, fileName, "pdf");
        //    //}

        //    //private void ConvertTo(string repositoryPath, string fileName, string format)
        //    //{
        //    //    string svgFileName = Path.ChangeExtension(fileName, format);
        //    //    string arguments = string.Format("{0} -T{2} -o{1}", fileName, svgFileName, format);
        //    //    ProcessStartInfo psi = new ProcessStartInfo();
        //    //    psi.Arguments = arguments;
        //    //    psi.FileName = _appPathProvider.GetProperAppPath(ExternalApp.GraphViz);
        //    //    psi.WorkingDirectory = repositoryPath;
        //    //    Process.Start(psi);
        //}
    }
}