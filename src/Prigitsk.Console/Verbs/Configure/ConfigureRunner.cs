using Microsoft.Extensions.Logging;

namespace Prigitsk.Console.Verbs.Configure
{
    public class ConfigureRunner : VerbRunnerBase<IConfigureRunnerOptions>, IConfigureRunner
    {
        public ConfigureRunner(IConfigureRunnerOptions options, ILogger log) : base(options, log: log)
        {
        }

        protected override void RunInternal()
        {
        }
    }
}