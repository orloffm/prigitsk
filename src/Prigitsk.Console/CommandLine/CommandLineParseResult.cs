using Prigitsk.Console.Verbs;

namespace Prigitsk.Console.CommandLine
{
    public class CommandLineParseResult
    {
        public bool IsCorrect { get; set; }
        public Verb Verb { get; set; }
        public object VerbOptions { get; set; }
    }
}