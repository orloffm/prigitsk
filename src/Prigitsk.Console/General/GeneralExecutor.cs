﻿using System;
using Autofac.Features.Indexed;
using Microsoft.Extensions.Logging;
using Prigitsk.Console.CommandLine;
using Prigitsk.Console.Verbs;

namespace Prigitsk.Console.General
{
    /// <summary>
    ///     General class that handles execution.
    /// </summary>
    public class GeneralExecutor : IGeneralExecutor
    {
        private readonly IIndex<Verb, IVerbRunnerFactory> _factorySelector;
        private readonly ILogger _log;
        private readonly ICommandLineParser _parser;

        public GeneralExecutor(ICommandLineParser parser, IIndex<Verb, IVerbRunnerFactory> factorySelector, ILogger log)
        {
            _parser = parser;
            _factorySelector = factorySelector;
            _log = log;
        }

        public int RunSafe(string[] args)
        {
            try
            {
                return RunInternal(args: args);
            }
            catch (LoggedAsFatalException)
            {
                _log.Fatal("Aborting execution.");
                return 1;
            }
            catch (Exception ex)
            {
                _log.Fatal(ex: ex, message: "Unhandled exception occurred. Aborting execution.");
                return 1;
            }
        }

        private int RunInternal(string[] args)
        {
            _log.Trace("Application starting.");

            CommandLineParseResult parseResult = _parser.Parse(args: args);
            if (!parseResult.IsCorrect || !parseResult.Verb.HasValue)
            {
                _log.Error("Command line arguments incorrect.");
                return 1;
            }

            IVerbRunner verbRunner = CreateVerbRunner(verb: parseResult.Verb.Value, options: parseResult.VerbOptions);
            verbRunner.Run();

            return 0;
        }

        private IVerbRunner CreateVerbRunner(Verb verb, IVerbRunnerOptions options)
        {
            IVerbRunnerFactory factory = _factorySelector[key: verb];
            IVerbRunner runner = factory.Create(verbOptions: options);
            return runner;
        }
    }
}