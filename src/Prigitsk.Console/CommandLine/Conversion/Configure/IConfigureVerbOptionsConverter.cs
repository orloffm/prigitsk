using Prigitsk.Console.CommandLine.Parsing;
using Prigitsk.Console.Verbs.Configure;

namespace Prigitsk.Console.CommandLine.Conversion.Configure
{
    public interface IConfigureVerbOptionsConverter : IVerbOptionsConverter<ConfigureOptions, IConfigureRunnerOptions>
    {
    }
}