using Microsoft.Extensions.Logging;
using Prigitsk.Console.CommandLine.Parsing;
using Prigitsk.Console.Verbs.Draw;

namespace Prigitsk.Console.CommandLine.Conversion.Draw
{
    public class DrawVerbOptionsConverter
        : VerbOptionsConverterBase<DrawOptions, IDrawRunnerOptions>, IDrawVerbOptionsConverter
    {
        public DrawVerbOptionsConverter(ILogger log) : base(log)
        {
        }

        protected override IDrawRunnerOptions ConvertOptionsInternal(DrawOptions source)
        {
            return new DrawRunnerOptions(
                source.Repository,
                source.Target,
                source.Output,
                source.Format,
                source.RemoteToUse,
                source.ForceTreatAsGitHub,
                source.LeaveTails
            );
        }
    }
}