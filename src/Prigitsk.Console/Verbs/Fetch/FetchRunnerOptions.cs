namespace Prigitsk.Console.Verbs.Fetch
{
    public class FetchRunnerOptions : IFetchRunnerOptions
    {
        public FetchRunnerOptions(string url = null, string repository = null)
        {
            Url = url;
            Repository = repository;
        }

        public string Url { get; }
        public string Repository { get; }
    }
}