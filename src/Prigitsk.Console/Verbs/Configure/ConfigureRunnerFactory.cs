using System;
using Microsoft.Extensions.Logging;

namespace Prigitsk.Console.Verbs.Configure
{
    public class ConfigureRunnerFactory : VerbRunnerFactoryBase<IConfigureRunnerOptions>
    {
        private readonly Func<IConfigureRunnerOptions, IConfigureRunner> _maker;

        public ConfigureRunnerFactory(ILogger log, Func<IConfigureRunnerOptions, IConfigureRunner> maker) : base(log)
        {
            _maker = maker;
        }

        protected override IVerbRunner<IConfigureRunnerOptions> CreateInternal(IConfigureRunnerOptions options)
        {
            return _maker.Invoke(options);
        }
    }
}