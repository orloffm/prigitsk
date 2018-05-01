using Microsoft.Extensions.Logging;
using Prigitsk.Console.CommandLine.Parsing;
using Prigitsk.Console.Verbs.Fetch;

namespace Prigitsk.Console.CommandLine.Conversion.Fetch
{
    public class FetchVerbOptionsConverter
        : VerbOptionsConverterBase<FetchOptions, IFetchRunnerOptions>, IFetchVerbOptionsConverter
    {
        public FetchVerbOptionsConverter(ILogger<FetchVerbOptionsConverter> log) : base(log)
        {
        }

        protected override IFetchRunnerOptions ConvertOptionsInternal(FetchOptions source)
        {
            return new FetchRunnerOptions(
                source.Url,
                source.Repository);
        }
    }
}