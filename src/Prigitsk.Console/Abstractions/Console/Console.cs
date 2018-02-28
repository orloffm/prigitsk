using Microsoft.Extensions.Logging;
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

        private void LogWritten(string text)
        {
            _log.Log(_level, $"Console out: {text}");
        }

        public string ReadLine()
        {
            _log.Log(_level, $"Console in starting...");
            string text = SysConsole.ReadLine();
            _log.Log(_level, $"Console in: {text}");
            return text;
        }

        public void Write(string text)
        {
            SysConsole.Write(text);
            LogWritten(text);
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
                WriteLine(string.Format(format, arg));
            }
        }

        public void WriteLine(string text)
        {
            SysConsole.WriteLine(text);
            LogWritten(text);
        }
    }
}