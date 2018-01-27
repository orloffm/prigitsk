using Microsoft.Extensions.Logging;

namespace Prigitsk.Console.Abstractions.Console
{
    public interface IConsoleFactory
    {
        /// <summary>
        /// Returns a console wrapper with the appropriate logging level.
        /// </summary>
        IConsole Create(LogLevel? verbosityLevel = LogLevel.Debug);
    }
}