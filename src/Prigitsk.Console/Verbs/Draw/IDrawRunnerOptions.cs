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

        string Format { get; }

        /// <summary>
        ///     Prevent concatenation of nodes on branches after last merge or diversion.
        ///     In other words, this leaves all final direct commits on a branch untouched.
        /// </summary>
        bool LeaveTails { get; }

        /// <summary>
        ///     Output file name.
        /// </summary>
        string OutputFileName { get; }

        /// <summary>
        ///     Which remote to use to build the graph.
        ///     Default means origin or the single existing one.
        /// </summary>
        string RemoteToUse { get; }

        string Repository { get; }

        /// <summary>
        ///     Target directory.
        /// </summary>
        string TargetDirectory { get; }
    }
}