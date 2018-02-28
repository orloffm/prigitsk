using Prigitsk.Console.CommandLine.Parsing;
using Prigitsk.Console.Verbs.Fetch;

namespace Prigitsk.Console.CommandLine.Conversion.Fetch
{
    public interface IFetchVerbOptionsConverter : IVerbOptionsConverter<FetchOptions, IFetchRunnerOptions>
    {
    }
}