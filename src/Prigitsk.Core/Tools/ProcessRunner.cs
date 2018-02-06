using System.Diagnostics;

namespace Prigitsk.Core.Tools
{
    public class ProcessRunner : IProcessRunner
    {
        public string Execute(
            string command,
            string argument)
        {
            string executeResult = string.Empty;
            Process executeProcess = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    FileName = command,
                    Arguments = argument,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            executeProcess.Start();
            executeResult = executeProcess.StandardOutput.ReadToEnd();
            executeProcess.WaitForExit();
            if (executeProcess.ExitCode == 0)
            {
                return executeResult;
            }

            return string.Empty;
        }
    }
}