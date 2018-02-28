using CommandLine;
using Prigitsk.Console.General;

namespace Prigitsk.Console.CommandLine.Parsing
{
    [Verb(VerbConstants.Configure, HelpText = "Configure the application settings.")]
    public class ConfigureOptions : IVerbOptions
    {
    }
}