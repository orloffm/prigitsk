using CommandLine;

namespace Prigitsk.Console.CommandLine
{
    [Verb("fetch", HelpText = "Clone or fetch the remote repository.")]
    public class FetchOptions
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