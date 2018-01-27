using Microsoft.Extensions.Logging;

namespace Prigitsk.Console.Abstractions
{
    public sealed class WindowsEnvironment : IWindowsEnvironment
    {
        private readonly ILogger _log;

        public WindowsEnvironment(ILogger log)
        {
            _log = log;
        }

        public string GetEnvironmentVariable(string variable)
        {
            string value = System.Environment.GetEnvironmentVariable(variable);
            _log.Debug("Environment variable {0} = '{1}'.", variable, value);
            return value;
        }
    }
}