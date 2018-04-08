namespace Prigitsk.Console.Verbs.Draw
{
    public interface IDrawRunnerOptions
        : IVerbRunnerOptions
    {
        string Format { get; }

        /// <summary>
        ///     Output file name.
        /// </summary>
        string OutputFileName { get; }

        /// <summary>
        ///     Which remote to use to build the graph. Default means origin or the single existing one.
        /// </summary>
        string RemoteToUse { get; }

        string Repository { get; }

        /// <summary>
        ///     Target directory.
        /// </summary>
        string TargetDirectory { get; }
    }
}