using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Prigitsk.Core.Tools;

namespace Prigitsk.Console.Tools
{
    public sealed class ProcessRunner : IProcessRunner
    {
        private readonly ILogger _log;

        public ProcessRunner(ILogger<ProcessRunner> log)
        {
            _log = log;
        }

        public int Execute(
            string command,
            string argument)
        {
            Process executeProcess = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = command,
                    Arguments = argument,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            executeProcess.Start();
            string outResult = executeProcess.StandardOutput.ReadToEnd();
            string errorResult = executeProcess.StandardError.ReadToEnd();
            executeProcess.WaitForExit();
            if (!string.IsNullOrWhiteSpace(outResult))
            {
                _log.Info(outResult);
            }

            if (!string.IsNullOrWhiteSpace(errorResult))
            {
                _log.Error(errorResult);
            }

            return executeProcess.ExitCode;
        }
    }
}