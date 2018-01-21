using System;
using Microsoft.Extensions.Logging;
using Prigitsk.Console.CommandLine.Parsing;
using Prigitsk.Console.Verbs.Fetch;

namespace Prigitsk.Console.CommandLine.Conversion.Fetch
{
    public class FetchVerbOptionsConverterFactory : VerbOptionsConverterFactoryBase<FetchOptions, IFetchRunnerOptions>
    {
        private readonly Func<IFetchVerbOptionsConverter> _maker;

        public FetchVerbOptionsConverterFactory(ILogger log, Func<IFetchVerbOptionsConverter> maker) : base(log)
        {
            _maker = maker;
        }

        protected override IVerbOptionsConverter<FetchOptions, IFetchRunnerOptions> CreateInternal()
        {
            return   _maker.Invoke();
        }
    }
}