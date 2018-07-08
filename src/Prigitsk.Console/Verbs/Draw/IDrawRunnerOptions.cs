using Prigitsk.Core.Graph;
using Prigitsk.Core.Strategy;

namespace Prigitsk.Console.Verbs.Draw
{
    public interface IDrawRunnerOptions
        : IVerbRunnerOptions
    {
        /// <summary>
        ///     Forcefully treat the repository as belonging to GitHub server when rendering.
        ///     Affects the links.
        /// </summary>
        bool ForceTreatAsGitHub { get; }

        /// <summary>
        ///     Output file format.
        /// </summary>
        string Format { get; }

        /// <summary>
        ///     Include tags attached to commits not accessible from any tip.
        ///     By default they are removed, and the tag count applies to the remaining tags.
        /// </summary>
        bool IncludeOrphanedTags { get; }

        /// <summary>
        ///     Do not remove commits not accessible from any tip.
        /// </summary>
        bool KeepAllOrphans { get; }

        /// <summary>
        ///     Prevent concatenation of nodes on branches after last merge or diversion.
        ///     In other words, this leaves all final direct commits on a branch untouched.
        /// </summary>
        bool LeaveHeads { get; }

        /// <summary>
        ///     Output file name.
        /// </summary>
        string OutputFileName { get; }

        /// <summary>
        ///     Prevent simplification of the tree. All commits will be preserved in the output.
        /// </summary>
        bool PreventSimplification { get; }

        /// <summary>
        ///     Which remote to use to build the graph.
        ///     Default means origin or the single existing one.
        /// </summary>
        string RemoteToUse { get; }

        /// <summary>
        ///     Do not treat the first node of the branch specially. It may get removed as other nodes.
        /// </summary>
        bool RemoveTails { get; }

        /// <summary>
        ///     Directory containing a .git repository.
        /// </summary>
        string Repository { get; }

        int TagCount { get; }

        TagPickingMode TagPickingMode { get; }

        /// <summary>
        ///     Target directory.
        /// </summary>
        string TargetDirectory { get; }

        /// <summary>
        ///     <see cref="IWorkItemSuffixRegex" />.
        /// </summary>
        IWorkItemSuffixRegex WorkItemBranchesRegex { get; }
    }
}