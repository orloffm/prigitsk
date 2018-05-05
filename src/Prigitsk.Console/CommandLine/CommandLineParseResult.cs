using Prigitsk.Console.General;
using Prigitsk.Console.Verbs;

namespace Prigitsk.Console.CommandLine
{
    public sealed class CommandLineParseResult
    {
        public static CommandLineParseResult Failed => new CommandLineParseResult {IsCorrect = false};

        public bool IsCorrect { get; private set; }

        public Verb? Verb { get; private set; }

        public IVerbRunnerOptions VerbOptions { get; private set; }

        public static CommandLineParseResult Correct(Verb verb, IVerbRunnerOptions options)
        {
            return new CommandLineParseResult {IsCorrect = true, Verb = verb, VerbOptions = options};
        }
    }
}