using System;
using Microsoft.Extensions.Logging;

namespace Prigitsk.Console.Abstractions.Console
{
    public class ConsoleFactory : IConsoleFactory
    {
        private readonly Func<LogLevel?, IConsole> _consoleMaker;

        public ConsoleFactory(Func<LogLevel?, IConsole> consoleMaker)
        {
            _consoleMaker = consoleMaker;
        }

        /// <inheritdoc />
        public IConsole Create(LogLevel? verbosityLevel)
        {
            return _consoleMaker(verbosityLevel);
        }
    }
}