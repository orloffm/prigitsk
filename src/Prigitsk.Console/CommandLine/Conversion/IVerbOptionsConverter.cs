using Prigitsk.Console.CommandLine.Parsing;
using Prigitsk.Console.Verbs;

namespace Prigitsk.Console.CommandLine.Conversion
{
    public interface IVerbOptionsConverter<TSource, TTarget> : IVerbOptionsConverter
        where TSource : IVerbOptions
        where TTarget : IVerbRunnerOptions
    {
    }

    public interface IVerbOptionsConverter
    {
        IVerbRunnerOptions ConvertOptions(IVerbOptions source);
    }
}