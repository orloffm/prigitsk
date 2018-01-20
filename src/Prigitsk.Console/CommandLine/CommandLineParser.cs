using System;
using Autofac.Features.Indexed;
using CommandLine;
using Microsoft.Extensions.Logging;
using Prigitsk.Console.Verbs;

namespace Prigitsk.Console.CommandLine
{
    
    public class CommandLineParser : ICommandLineParser
    {
        private readonly ILogger _log;

        public CommandLineParser(ILogger log, IVerbOptionsConverter verbOptionsConverter)
        {
            _log = log;
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
            else
            {
                CommandLineParseResult ret  = Command
               
                Parsed<object> parsed = (Parsed<object>)result;

                Verb v = GetVerbFrom(parsed);

                switch (v)
                {
                    case Verb.Configure:

                }

            }
            


        }
    }

    public interface IVerbOptionsConverter
    {
    }
}