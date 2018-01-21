using Microsoft.Extensions.Logging;
using Prigitsk.Console.CommandLine.Parsing;
using Prigitsk.Console.Verbs.Configure;

namespace Prigitsk.Console.CommandLine.Conversion.Configure
{
    public class ConfigureVerbOptionsConverter
        : VerbOptionsConverterBase<ConfigureOptions, IConfigureRunnerOptions>, IConfigureVerbOptionsConverter
    {
        public ConfigureVerbOptionsConverter(ILogger log) : base(log: log)
        {
        }

        protected override IConfigureRunnerOptions ConvertOptionsInternal(ConfigureOptions source)
        {
            return new ConfigureRunnerOptions();
        }
    }
}