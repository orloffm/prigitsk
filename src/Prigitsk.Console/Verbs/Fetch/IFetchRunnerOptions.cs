namespace Prigitsk.Console.Verbs.Fetch
{
    public interface IFetchRunnerOptions
        : IVerbRunnerOptions
    {
        string Repository { get; }

        string Url { get; }
    }
}