using System;
using Microsoft.Extensions.Logging;

namespace Prigitsk.Console.Verbs.Fetch
{
    public class FetchRunnerFactory : VerbRunnerFactoryBase<IFetchRunnerOptions>
    {
        private readonly Func<IFetchRunnerOptions, IFetchRunner> _maker;

        public FetchRunnerFactory(ILogger log, Func<IFetchRunnerOptions, IFetchRunner> maker) : base(log)
        {
            _maker = maker;
        }

        protected override IVerbRunner<IFetchRunnerOptions> CreateInternal(IFetchRunnerOptions options)
        {
            return _maker.Invoke(options);
        }
    }
}