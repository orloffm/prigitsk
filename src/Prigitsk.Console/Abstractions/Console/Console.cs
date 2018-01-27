using System;
using System.ServiceModel.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using SysConsole = System.Console;

namespace Prigitsk.Console.Abstractions.Console
{
    public class Console : IConsole
    {
        private readonly LogLevel? _level;
        private readonly ILogger _log;

        public Console(LogLevel? level, ILogger log)
        {
            _level = level;
            _log = log;
        }

        public void WriteLine()
        {
            SysConsole.WriteLine();
            LogWritten("<empty line>");
        }

        public void WriteLine(string format, params object[] arg)
        {
            if (arg == null)
            {
                WriteLine(format);
            }
            else
            {
                WriteLine(String.Format(format, arg));
            }
        }

        public void WriteLine(string text)
        {
                SysConsole.WriteLine(text);
            LogWritten(text);
        }

        private void LogWritten(string text)
        {
            _log.Log(_level, $"Console out: {text}");
        }
    }
}