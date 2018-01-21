using System;
using Microsoft.Extensions.Logging;
using Prigitsk.Console.CommandLine.Parsing;
using Prigitsk.Console.Verbs.Draw;

namespace Prigitsk.Console.CommandLine.Conversion.Draw
{
    public class DrawVerbOptionsConverterFactory : VerbOptionsConverterFactoryBase<DrawOptions, IDrawRunnerOptions>
    {
        private readonly Func<IDrawVerbOptionsConverter> _maker;

        public DrawVerbOptionsConverterFactory(ILogger log, Func<IDrawVerbOptionsConverter> maker) : base(log)
        {
            _maker = maker;
        }

        protected override IVerbOptionsConverter<DrawOptions, IDrawRunnerOptions> CreateInternal()
        {
            return   _maker.Invoke();
        }
    }
}