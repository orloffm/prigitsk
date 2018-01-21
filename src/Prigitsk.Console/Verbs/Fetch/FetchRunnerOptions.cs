namespace Prigitsk.Console.Verbs.Fetch
{
    public class FetchRunnerOptions : IFetchRunnerOptions
    {
        public FetchRunnerOptions(string url = null, string repository = null)
        {
            this.Url = url;
            this.Repository = repository;
        }

        public string Url { get; }
        public string Repository { get; }
    }
}