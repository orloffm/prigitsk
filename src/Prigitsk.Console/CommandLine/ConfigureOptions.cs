using CommandLine;
using Prigitsk.Console.Verbs;

namespace Prigitsk.Console.CommandLine
{
    [Verb(VerbConstants.Configure, HelpText = "Configure the application settings.")]
    public class ConfigureOptions : IVerbOptions
    {
    }
}