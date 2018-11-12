using System.Collections.Generic;
using CommandLine;
using Prigitsk.Console.General;

namespace Prigitsk.Console.CommandLine.Parsing
{
    [Verb(VerbConstants.Draw, HelpText = "Create a diagram of the branch structure.")]
    public class DrawOptions : IVerbOptions
    {
        [Option('e', "exclude", Separator = ',', HelpText = "Regular expressions for branches to exclude.")]
        public IEnumerable<string> ExcludeBranchesRegices { get; set; }

        [Option(
            'g',
            Default = false,
            HelpText =
                "Forcefully treat the repository as belonging to a GitHub server when rendering. Affects the links.")]
        public bool ForceTreatAsGitHub { get; set; }

        [Option("format", Default = "svg", HelpText = "Output file format.")]
        public string Format { get; set; }

        [Option('i', "include", Separator = ',', HelpText = "Regular expressions for branches to include.")]
        public IEnumerable<string> IncludeBranchesRegices { get; set; }

        [Option(
            "orphanedTags",
            Default = false,
            HelpText = "Include tags attached to commits not accessible from any tip. " +
                       "By default they are removed, and the tag count applies to the remaining tags.")]
        public bool IncludeOrphanedTags { get; set; }

        [Option(
            'k',
            "keepOrphans",
            Default = false,
            HelpText =
                "Do not remove orphan commits, i.e. those not accessible from any tip and not having any tags on them. " +
                "Tags on orphans can be removed by a separate argument.")]
        public bool KeepAllOrphans { get; set; }

        [Option(
            "leaveHeads",
            Default = false,
            HelpText =
                "Prevent concatenation of nodes on branches after last merge or diversion. In other words, this leaves all final direct commits on a branch untouched.")]
        public bool LeaveHeads { get; set; }

        [Option(
            "lesserBranchRegex",
            Default = @"",
            HelpText = "If this applies to a branch name, it is considered a lesser one and is drawn differently.")]
        public string LesserBranchesRegex { get; set; }

        /// <summary>
        ///     Disable writing tags.
        /// </summary>
        [Option(
            'n',
            "noTags",
            Default = false,
            HelpText = "Disable writing tags. Has higher priority than specifying tag count.")]
        public bool NoTags { get; set; }

        /// <summary>
        ///     The target file name.
        /// </summary>
        [Option('o', "output", HelpText = "Output file name.")]
        public string Output { get; set; }

        [Option(
            'p',
            "preventSimplify",
            Default = false,
            HelpText = "Prevent simplification of the tree. All commits will be preserved in the output.")]
        public bool PreventSimplification { get; set; }

        [Option(
            "remote",
            HelpText = "Remote to build the graph against. Origin or the single available is used by default.")]
        public string RemoteToUse { get; set; }

        [Option(
            "removeTails",
            Default = false,
            HelpText = "Do not treat the first node of the branch specially. It may get removed as other nodes.")]
        public bool RemoveTails { get; set; }

        /// <summary>
        ///     The repository directory.
        /// </summary>
        [Option('r', "repository", HelpText = "Directory containing a .git repository.")]
        public string Repository { get; set; }

        /// <summary>
        ///     The number of tags to include.
        /// </summary>
        [Option("tagCount", Default = -1, HelpText = "Number of latest tags to include.")]
        public int TagCount { get; set; }

        /// <summary>
        ///     The target directory.
        /// </summary>
        [Option('t', "target", HelpText = "Target directory.")]
        public string Target { get; set; }
    }
}