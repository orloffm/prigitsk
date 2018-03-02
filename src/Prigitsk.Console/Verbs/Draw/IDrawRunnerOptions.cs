namespace Prigitsk.Console.Verbs.Draw
{
    public interface IDrawRunnerOptions
        : IVerbRunnerOptions
    {
        string Format { get; }

        string Output { get; }

        /// <summary>
        ///     Which remote to use to build the graph. Default means origin or the single existing one.
        /// </summary>
        string RemoteToUse { get; }

        string Repository { get; }

        string Target { get; }
    }
}