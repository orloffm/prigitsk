using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ConGitWriter
{
    public class Worker : IWorker
    {
        private const string RepositoryPathArgumentName = "repository";
        private const string DotExeArgumentName = "dot";
        private const string GitExeArgumentName = "git";
        private const string TargetFileArgumentName = "target";
        private const string TargetDotFormatArgumentName = "format";

        public void Run(string[] args)
        {
            Dictionary<string, string> argsDic = ConsoleTools.ArgumentsToDictionary(args);

            if (!SettingsTools.LoadValue(RepositoryPathArgumentName, argsDic, Settings.Default, s => s.RepositoryDirectory))
                return;
            if (!SettingsTools.LoadValue(DotExeArgumentName, argsDic, Settings.Default, s => s.DotExePath))
                return;
            if (!SettingsTools.LoadValue(GitExeArgumentName, argsDic, Settings.Default, s => s.GitExePath))
                return;
            if (!SettingsTools.LoadValue(TargetFileArgumentName, argsDic, Settings.Default, s => s.TargetFilePath))
                return;
            if (!SettingsTools.LoadValue(TargetDotFormatArgumentName, argsDic, Settings.Default, s => s.TargetFormat))
                return;

            Settings.Default.Save();

            string repositoryPath = Settings.Default.RepositoryDirectory;
            string gitPath = Settings.Default.GitExePath;

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = gitPath;
            psi.Arguments = string.Format("--git-dir=\"{0}\" log --full-history --pretty=%h|%p|%d", repositoryPath);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;

            Process process = new Process();
            process.StartInfo = psi;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            string[] lines = output.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder dotData = new StringBuilder();
            dotData.AppendLine(@"strict digraph g
{
rankdir=""LR"";

");

            foreach (string s in lines)
            {
                string[] oneCommit = s.Split('|');
                string hash = oneCommit[0];
                string[] parentHashes = oneCommit[1].Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                string[] branches = oneCommit[2].Trim('(',')').Split(',');

                if (parentHashes.Length == 0)
                    dotData.AppendFormat("\"{0}\";\r\n", hash);
                else
                {
                    foreach (string parentHash in parentHashes)
                    {
                        dotData.AppendFormat("\"{0}\" -> \"{1}\";\r\n", parentHash, hash);
                    }
                }
            }

            dotData.AppendLine("}");

            string tempPath = Path.GetTempFileName();
            File.WriteAllText(tempPath, dotData.ToString());

            ProcessStartInfo dotPsi = new ProcessStartInfo();
            dotPsi.FileName = Settings.Default.DotExePath;
            dotPsi.Arguments = string.Format("-T{0} {1} -o\"{2}\"", Settings.Default.TargetFormat, tempPath, Settings.Default.TargetFilePath);
            dotPsi.CreateNoWindow = true;
            dotPsi.UseShellExecute = false;
            //dotPsi.RedirectStandardError = true;
            //dotPsi.RedirectStandardOutput = true;

            Process dotProcess = new Process();
            dotProcess.StartInfo = dotPsi;
            dotProcess.Start();

            //string dotResult = process.StandardOutput.ReadToEnd();
            dotProcess.WaitForExit();

            //File.WriteAllText(Settings.Default.TargetFilePath, dotResult);
        }
    }
}