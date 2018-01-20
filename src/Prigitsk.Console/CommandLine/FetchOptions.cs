using CommandLine;
using Prigitsk.Console.Verbs;

namespace Prigitsk.Console.CommandLine
{
    [Verb(VerbConstants.Fetch, HelpText = "Clone or fetch the remote repository.")]
    public class FetchOptions : IVerbOptions
    {
        /// <summary>
        ///     The repository directory.
        /// </summary>
        [Option('r', "repository", HelpText = "Directory containing a .git repository.")]
        public string Repository { get; set; }

        [Option('u', "url", HelpText = "Source URL to use.")]
        public string Url { get; set; }
    }
}