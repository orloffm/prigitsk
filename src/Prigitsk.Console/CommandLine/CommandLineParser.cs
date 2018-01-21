using System.Collections.Generic;
using System.Reflection;
using Autofac.Features.Indexed;
using CommandLine;
using Microsoft.Extensions.Logging;
using Prigitsk.Console.CommandLine.Conversion;
using Prigitsk.Console.CommandLine.Parsing;
using Prigitsk.Console.General;
using Prigitsk.Console.Verbs;

namespace Prigitsk.Console.CommandLine
{
    public class CommandLineParser : ICommandLineParser
    {
        private readonly IIndex<Verb, IVerbOptionsConverterFactory> _factories;
        private readonly ILogger _log;

        public CommandLineParser(ILogger log, IIndex<Verb, IVerbOptionsConverterFactory> factories)
        {
            _log = log;
            _factories = factories;
        }

        public CommandLineParseResult Parse(string[] args)
        {
            ParserResult<object> result =
                Parser.Default.ParseArguments<ConfigureOptions, DrawOptions, FetchOptions>(args: args);
            if (result is NotParsed<object>)
            {
                IEnumerable<Error> errors = (result as NotParsed<object>).Errors;
                foreach (Error error in errors)
                {
                    _log.Error("Invalid command line arguments ({0}).", error.Tag);
                }

                return CommandLineParseResult.Failed;
            }

            var parsed = (Parsed<object>) result;
            IVerbOptions options = parsed.Value as IVerbOptions;
            if (options == null)
            {
                string message = "Unexpected problem parsing command line.";
                _log.Fatal(message: message);
                throw new LoggedAsFatalException(message: message);
            }

            Verb v = GetVerbFrom(parsed: options);
            IVerbOptionsConverterFactory converterFactory = _factories[key: v];
            IVerbOptionsConverter converter = converterFactory.Create();
            IVerbRunnerOptions runnerOptions = converter.ConvertOptions(source: options);

            return CommandLineParseResult.Correct(verb: v, options: runnerOptions);
        }

        private Verb GetVerbFrom(IVerbOptions parsed)
        {
            VerbAttribute verbAttribute = parsed.GetType().GetCustomAttribute<VerbAttribute>();
            string verbName = verbAttribute.Name;
            Verb verb = VerbHelper.FromName(verbName: verbName);
            return verb;
        }
    }
}