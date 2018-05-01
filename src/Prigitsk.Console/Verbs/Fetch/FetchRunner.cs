using Microsoft.Extensions.Logging;

namespace Prigitsk.Console.Verbs.Fetch
{
    public sealed class FetchRunner : VerbRunnerBase<IFetchRunnerOptions>, IFetchRunner
    {
        public FetchRunner(IFetchRunnerOptions options, ILogger<FetchRunner> log) : base(options, log)
        {
        }

        protected override void RunInternal()
        {
            Log.Info("Fetch runner starting.");
        }
    }
}