using CommandLine;
using Prigitsk.Console.General;

namespace Prigitsk.Console.CommandLine.Parsing
{
    [Verb(VerbConstants.Draw, HelpText = "Create a diagram of the branch structure.")]
    public class DrawOptions : IVerbOptions
    {
        [Option(
            'g',
            Default = false,
            HelpText =
                "Forcefully treat the repository as belonging to a GitHub server when rendering. Affects the links.")]
        public bool ForceTreatAsGitHub { get; set; }

        [Option('f', "format", HelpText = "Output file format.")]
        public string Format { get; set; }

        [Option(
            "leaveTails",
            Default = false,
            HelpText =
                "Prevent concatenation of nodes on branches after last merge or diversion. In other words, this leaves all final direct commits on a branch untouched.")]
        public bool LeaveTails { get; set; }

        /// <summary>
        ///     The target file name.
        /// </summary>
        [Option('o', "output", HelpText = "Output file name.")]
        public string Output { get; set; }

        [Option(
            "remote",
            HelpText = "Remote to build the graph against. Origin or the single available is used by default.")]
        public string RemoteToUse { get; set; }

        /// <summary>
        ///     The repository directory.
        /// </summary>
        [Option('r', "repository", HelpText = "Directory containing a .git repository.")]
        public string Repository { get; set; }

        /// <summary>
        ///     The target directory.
        /// </summary>
        [Option('t', "target", HelpText = "Target directory.")]
        public string Target { get; set; }
    }
}