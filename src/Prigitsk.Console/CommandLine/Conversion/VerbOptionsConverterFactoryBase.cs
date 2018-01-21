using Microsoft.Extensions.Logging;
using Prigitsk.Console.CommandLine.Parsing;
using Prigitsk.Console.Verbs;

namespace Prigitsk.Console.CommandLine.Conversion
{
    public abstract class VerbOptionsConverterFactoryBase<TSource, TTarget> : IVerbOptionsConverterFactory
        where TSource : IVerbOptions
        where TTarget : IVerbRunnerOptions
    {
        protected VerbOptionsConverterFactoryBase(ILogger log)
        {
            Log = log;
        }

        protected ILogger Log { get; }

        public IVerbOptionsConverter Create()
        {
            return CreateInternal();
        }

        protected abstract IVerbOptionsConverter<TSource, TTarget> CreateInternal();
    }
}