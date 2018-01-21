using Prigitsk.Console.CommandLine.Parsing;
using Prigitsk.Console.Verbs.Draw;

namespace Prigitsk.Console.CommandLine.Conversion.Draw
{
    public interface IDrawVerbOptionsConverter : IVerbOptionsConverter<DrawOptions, IDrawRunnerOptions>
    {
    }
}