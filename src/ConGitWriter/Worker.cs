using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using OrlovMikhail.GitTools.Helpers;

namespace ConGitWriter
{
    public class Worker : IWorker
    {
        private readonly IConGitWriterSettingsWrapper _settings;
        private readonly ISettingsHelper _settingsHelper;
        private readonly IConsoleArgumentsHelper _consoleHelper;
        private const string RepositoryPathArgumentName = "repository";
        private const string DotExeArgumentName = "dot";
        private const string GitExeArgumentName = "git";
        private const string TargetFileArgumentName = "target";
        private const string TargetDotFormatArgumentName = "format";

        public Worker(IConGitWriterSettingsWrapper settings, ISettingsHelper settingsHelper, IConsoleArgumentsHelper consoleHelper)
        {
            _settings = settings;
            _settingsHelper = settingsHelper;
            _consoleHelper = consoleHelper;
        }

        public void Run(string[] args)
        {
            Dictionary<string, string> argsDic = _consoleHelper.ArgumentsToDictionary(args);

            bool correct = true;
            correct &= _settingsHelper.UpdateFrom(argsDic, RepositoryPathArgumentName, _settings, s => s.RepositoryDirectory);
            correct &= _settingsHelper.UpdateFrom(argsDic, DotExeArgumentName, _settings, s => s.DotExePath);
            correct &= _settingsHelper.UpdateFrom(argsDic, GitExeArgumentName, _settings, s => s.GitExePath);
            correct &= _settingsHelper.UpdateFrom(argsDic, TargetFileArgumentName, _settings, s => s.TargetFilePath);
            correct &= _settingsHelper.UpdateFrom(argsDic, TargetDotFormatArgumentName, _settings, s => s.TargetFormat);

            if (!correct)
                return;

            _settings.Save();

            string repositoryPath = _settings.RepositoryDirectory;
            string gitPath = _settings.GitExePath;

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
            dotPsi.FileName = _settings.DotExePath;
            dotPsi.Arguments = string.Format("-T{0} {1} -o\"{2}\"", _settings.TargetFormat, tempPath, _settings.TargetFilePath);
            dotPsi.CreateNoWindow = true;
            dotPsi.UseShellExecute = false;
            //dotPsi.RedirectStandardError = true;
            //dotPsi.RedirectStandardOutput = true;

            Process dotProcess = new Process();
            dotProcess.StartInfo = dotPsi;
            dotProcess.Start();

            //string dotResult = process.StandardOutput.ReadToEnd();
            dotProcess.WaitForExit();

            //File.WriteAllText(_settings.TargetFilePath, dotResult);
        }
    }
    
}