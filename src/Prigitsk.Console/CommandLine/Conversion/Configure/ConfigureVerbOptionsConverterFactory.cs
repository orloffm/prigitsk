using System;
using Microsoft.Extensions.Logging;
using Prigitsk.Console.CommandLine.Parsing;
using Prigitsk.Console.Verbs.Configure;

namespace Prigitsk.Console.CommandLine.Conversion.Configure
{
    public class ConfigureVerbOptionsConverterFactory : VerbOptionsConverterFactoryBase<ConfigureOptions, IConfigureRunnerOptions>
    {
        private readonly Func<IConfigureVerbOptionsConverter> _maker;

        public ConfigureVerbOptionsConverterFactory(ILogger log, Func<IConfigureVerbOptionsConverter> maker) : base(log)
        {
            _maker = maker;
        }

        protected override IVerbOptionsConverter<ConfigureOptions, IConfigureRunnerOptions> CreateInternal()
        {
            return   _maker.Invoke();
        }
    }
}