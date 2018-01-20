using System;
using Microsoft.Extensions.Logging;

namespace Prigitsk.Console.Verbs.Fetch
{
    public class FetchRunner : VerbRunnerBase<IFetchRunnerOptions>, IFetchRunner
    {
        public FetchRunner(IFetchRunnerOptions options, ILogger log):base(options,log)
        {
        }

        public override void Run()
        {
            
        }
    }
}