using CommandLine;
using Prigitsk.Console.General;

namespace Prigitsk.Console.CommandLine.Parsing
{
    [Verb(VerbConstants.Draw, HelpText = "Create a diagram of the branch structure.")]
    public class DrawOptions : IVerbOptions
    {
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

        /// <summary>
        ///     The target file name.
        /// </summary>
        [Option('o', "output", HelpText = "Output file name.")]
        public string Output { get; set; }

        [Option('f', "format", HelpText = "Output file format.")]
        public string Format { get; set; }
    }
}