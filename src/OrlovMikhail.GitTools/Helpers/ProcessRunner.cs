using System.Diagnostics;

namespace OrlovMikhail.GitTools.Helpers
{
    public class ProcessRunner : IProcessRunner
    {
        /// <inheritdoc />
        public string Run(string executable, string arguments)
        {
            ProcessStartInfo dotPsi = new ProcessStartInfo
            {
                FileName = executable,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            Process dotProcess = new Process {StartInfo = dotPsi};
            dotProcess.Start();

            string dotResult = dotProcess.StandardOutput.ReadToEnd();
            dotProcess.WaitForExit();

            return dotResult;
        }
    }
}