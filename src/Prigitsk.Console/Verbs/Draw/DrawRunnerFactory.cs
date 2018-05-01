using System;
using Microsoft.Extensions.Logging;

namespace Prigitsk.Console.Verbs.Draw
{
    public sealed class DrawRunnerFactory : VerbRunnerFactoryBase<IDrawRunnerOptions>
    {
        private readonly Func<IDrawRunnerOptions, IDrawRunner> _maker;

        public DrawRunnerFactory(ILogger<DrawRunnerFactory> log, Func<IDrawRunnerOptions, IDrawRunner> maker) : base(
            log)
        {
            _maker = maker;
        }

        protected override IVerbRunner<IDrawRunnerOptions> CreateInternal(IDrawRunnerOptions options)
        {
            return _maker.Invoke(options);
        }
    }
}