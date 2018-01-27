﻿using Microsoft.Extensions.Logging;

namespace Prigitsk.Console.Verbs.Fetch
{
    public class FetchRunner : VerbRunnerBase<IFetchRunnerOptions>, IFetchRunner
    {
        public FetchRunner(IFetchRunnerOptions options, ILogger log) : base(options, log)
        {
        }

        protected override void RunInternal()
        {
            Log.Info("Fetch runner starting.");
        }
    }
}