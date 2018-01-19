using System.Diagnostics;

namespace Prigitsk.Core.Tools
{
    public class ProcessRunner : IProcessRunner
    {
        public string Execute(
            string command,
            string argument)
        {
            string ExecuteResult = string.Empty;
            Process ExecuteProcess = new Process
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
            ExecuteProcess.Start();
            ExecuteResult = ExecuteProcess.StandardOutput.ReadToEnd();
            ExecuteProcess.WaitForExit();
            if (ExecuteProcess.ExitCode == 0)
            {
                return ExecuteResult;
            }
            return string.Empty;
        }
    }
}