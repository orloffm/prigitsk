using Microsoft.Extensions.Logging;
using Prigitsk.Console.CommandLine.Parsing;
using Prigitsk.Console.Verbs;

namespace Prigitsk.Console.CommandLine.Conversion
{
    public abstract class VerbOptionsConverterBase<TSource, TTarget> : IVerbOptionsConverter<TSource, TTarget>
        where TSource : IVerbOptions
        where TTarget : IVerbRunnerOptions
    {
        protected VerbOptionsConverterBase( ILogger log)
        {
            Log = log;
        }

        protected ILogger Log { get; }

        public IVerbRunnerOptions ConvertOptions(IVerbOptions source)
        {
            TTarget result = ConvertOptionsInternal((TSource) source);
            return result;
        }

        protected abstract TTarget ConvertOptionsInternal(TSource source);
    }
}