namespace Prigitsk.Console.Verbs.Fetch
{
    public class FetchRunnerOptions : IFetchRunnerOptions
    {
        public FetchRunnerOptions(string url = null, string repository = null)
        {
            Url = url;
            Repository = repository;
        }

        public string Repository { get; }

        public string Url { get; }
    }
}