using System;
using Autofac;
using Microsoft.Extensions.Logging;
using Prigitsk.Console.Verbs.Configure;
using Prigitsk.Console.Verbs.Draw;
using Prigitsk.Console.Verbs.Fetch;

namespace Prigitsk.Console.Verbs
{
    public class VerbRunnerFactory : IVerbRunnerFactory
    {
        private readonly ILogger _log;
        private readonly Func<IDrawRunnerOptions, IDrawRunner> _drawRunnerFactory;
        private readonly Func<IFetchRunnerOptions, IFetchRunner> _fetchRunnerFactory;
        private readonly Func<IConfigureRunnerOptions, IConfigureRunner> _configureRunnerFactory;

        public VerbRunnerFactory(
            Func<IDrawRunnerOptions, IDrawRunner> drawRunnerFactory,
       // Func<IFetchRunnerOptions, IFetchRunner> fetchRunnerFactory,
      //  Func<IConfigureRunnerOptions, IConfigureRunner> configureRunnerFactory,
                ILogger log)
        {
            _log = log;
            _drawRunnerFactory = drawRunnerFactory;
       //     _fetchRunnerFactory = fetchRunnerFactory;
       //     _configureRunnerFactory = configureRunnerFactory;
        }

        public IVerbRunner CreateRunner(Verb verb, object verbOptions)
        {
            switch (verb)
            {
                case Verb.Configure:
                    return CreateRunnerInternal<IConfigureRunner, IConfigureRunnerOptions>(_configureRunnerFactory, verbOptions);
                case Verb.Fetch:
                    return CreateRunnerInternal<IFetchRunner, IFetchRunnerOptions>(_fetchRunnerFactory, verbOptions);
                case Verb.Draw:
                    return CreateRunnerInternal<IDrawRunner, IDrawRunnerOptions>(_drawRunnerFactory, verbOptions);
                default:
                    throw new ArgumentOutOfRangeException(nameof(verb), verb, null);
            }
        }

        private TRunner CreateRunnerInternal<TRunner, TOptions>(Func<TOptions, TRunner> factory, object verbOptions)
            where TRunner : IVerbRunner<TOptions>
            where TOptions : class, IVerbRunnerOptions
        {
            _log.Trace("Resolving a {0} runner.", typeof(TRunner).Name);

            TOptions optionsTyped = verbOptions as TOptions;
            if (optionsTyped == null)
            {
                _log.Warn(
                    "No correct options of type {0} provided when generating a runner of type {1}.",
                    typeof(TOptions).Name,
                    typeof(TRunner).Name);
            }

            TRunner runner = factory.Invoke(optionsTyped);
            return runner;
        }
    }
}