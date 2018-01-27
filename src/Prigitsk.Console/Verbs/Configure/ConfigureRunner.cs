using Microsoft.Extensions.Logging;
using Prigitsk.Console.Settings;

namespace Prigitsk.Console.Verbs.Configure
{
    public class ConfigureRunner : VerbRunnerBase<IConfigureRunnerOptions>, IConfigureRunner
    {
        private readonly ISettingsWrapper _settings;

        public ConfigureRunner(IConfigureRunnerOptions options, ISettingsWrapper settings, ILogger log) : base(options, log: log)
        {
            _settings = settings;
        }

        protected override void RunInternal()
        {
            
        }
    }
}