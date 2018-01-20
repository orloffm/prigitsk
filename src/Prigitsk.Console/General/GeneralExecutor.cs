using System;
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
        private readonly ILogger _log;
        private readonly ICommandLineParser _parser;
        private readonly IVerbRunnerFactory _verbRunnerFactory;

        public GeneralExecutor(ICommandLineParser parser, IVerbRunnerFactory verbRunnerFactory, ILogger log)
        {
            _parser = parser;
            _verbRunnerFactory = verbRunnerFactory;
            _log = log;
        }

        public int RunSafe(string[] args)
        {
            try
            {
                return RunInternal(args);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return 1;
            }
        }

        private int RunInternal(string[] args)
        {
            _log.Trace("Application starting.");

            CommandLineParseResult parseResult = _parser.Parse(args);
            if (!parseResult.IsCorrect)
            {
                _log.Trace("Command line arguments incorrect.");
                return 1;
            }

            IVerbRunner verbRunner = CreateVerbRunner(parseResult);
            verbRunner.Run();

            return 0;
        }

        private IVerbRunner CreateVerbRunner(CommandLineParseResult parseResult)
        {
            return _verbRunnerFactory.CreateRunner(parseResult.Verb, parseResult.VerbOptions);
        }
    }
}