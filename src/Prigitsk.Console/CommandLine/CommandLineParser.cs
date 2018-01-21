using System;
using System.Reflection;
using Autofac.Features.Indexed;
using CommandLine;
using Microsoft.Extensions.Logging;
using Prigitsk.Console.CommandLine.Conversion;
using Prigitsk.Console.CommandLine.Parsing;
using Prigitsk.Console.General;
using Prigitsk.Console.Verbs;
using Prigitsk.Console.Verbs.Draw;

namespace Prigitsk.Console.CommandLine
{
    
    public class CommandLineParser : ICommandLineParser
    {
        private readonly ILogger _log;
        private readonly IIndex<Verb, IVerbOptionsConverterFactory> _factories;

        public CommandLineParser(ILogger log, IIndex<Verb, IVerbOptionsConverterFactory> factories)
        {
            _log = log;
            _factories = factories;
        }

        public CommandLineParseResult Parse(string[] args)
        {
            

            var result = Parser.Default.ParseArguments<ConfigureOptions, DrawOptions, FetchOptions>(args);
            if (result is NotParsed<object>)
            {
                var errors = (result as NotParsed<object>).Errors;
                foreach (Error error in errors)
                {
                    _log.Error("Invalid command line arguments ({0}).", error.Tag);
                }

                return CommandLineParseResult.Failed;
            }

            Parsed<object> parsed = (Parsed<object>)result;
            IVerbOptions options = parsed.Value as IVerbOptions;
            if (options == null)
            {
                string message = "Unexpected problem parsing command line.";
                _log.Fatal(message);
                throw new LoggedAsFatalException(message);
            }

            Verb v = GetVerbFrom(options);
            var converterFactory = _factories[v];
            var converter = converterFactory.Create();
            IVerbRunnerOptions runnerOptions = converter.ConvertOptions(options);
                
            return CommandLineParseResult.Correct(v,runnerOptions);
        }

        private Verb GetVerbFrom(IVerbOptions parsed)
        {
            VerbAttribute verbAttribute =  parsed.GetType().GetCustomAttribute<VerbAttribute>();
            string verbName = verbAttribute.Name;
            Verb verb = VerbHelper.FromName(verbName);
            return verb;
        }
    }

}