namespace Prigitsk.Console.CommandLine
{
    public interface ICommandLineParser
    {
        CommandLineParseResult Parse(string[] args);
    }
}