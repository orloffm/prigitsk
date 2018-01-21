namespace Prigitsk.Console.Verbs.Fetch
{
    public interface IFetchRunnerOptions
        : IVerbRunnerOptions
    {
        string Url { get; }
        string Repository { get; }
    }
}