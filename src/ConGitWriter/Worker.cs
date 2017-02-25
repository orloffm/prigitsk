using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using OrlovMikhail.GitTools.Helpers;
using OrlovMikhail.GitTools.Loading;
using OrlovMikhail.GitTools.Loading.Client.Common;

namespace ConGitWriter
{
    public class Worker : IWorker
    {
        private readonly IConGitWriterSettingsWrapper _settings;
        private readonly ISettingsHelper _settingsHelper;
        private readonly IConsoleArgumentsHelper _consoleHelper;
        private readonly IGitClientFactory _gitClientFactory;
        private const string RepositoryPathArgumentName = "repository";
        private const string DotExeArgumentName = "dot";
        private const string GitExeArgumentName = "git";
        private const string TargetFileArgumentName = "target";
        private const string TargetDotFormatArgumentName = "format";

        public Worker(IConGitWriterSettingsWrapper settings,
            ISettingsHelper settingsHelper,
            IConsoleArgumentsHelper consoleHelper,
            IGitClientFactory gitClientFactory)
        {
            _settings = settings;
            _settingsHelper = settingsHelper;
            _consoleHelper = consoleHelper;
            _gitClientFactory = gitClientFactory;
        }

        public void Run(string[] args)
        {
            Dictionary<string, string> argsDic = _consoleHelper.ArgumentsToDictionary(args);

            bool correct = true;
            correct &= _settingsHelper.UpdateFrom(argsDic, RepositoryPathArgumentName, _settings,
                s => s.RepositoryDirectory);
            correct &= _settingsHelper.UpdateFrom(argsDic, DotExeArgumentName, _settings, s => s.DotExePath);
            correct &= _settingsHelper.UpdateFrom(argsDic, GitExeArgumentName, _settings, s => s.GitExePath);
            correct &= _settingsHelper.UpdateFrom(argsDic, TargetFileArgumentName, _settings, s => s.TargetFilePath);
            correct &= _settingsHelper.UpdateFrom(argsDic, TargetDotFormatArgumentName, _settings, s => s.TargetFormat);

            if (!correct)
            {
                return;
            }

            _settings.Save();

            string repositoryPath = _settings.RepositoryDirectory;

            IRepositoryData data;

            using (IGitClient client = _gitClientFactory.CreateClient(repositoryPath))
            {
                client.Init();

                data = client.Load();
            }


            StringBuilder dotData = new StringBuilder();
            dotData.AppendLine(@"strict digraph g
{
rankdir=""LR"";

");

            //using (var repo = new Repository(repositoryPath))
            //{
            //    foreach (CommitInfo c in repo.Commits)
            //    {
            //        string hash = getSha (c);
            //        string[] parentHashes = c.Parents.Select(getSha).ToArray();

            //        if (parentHashes.Length == 0)
            //            dotData.AppendFormat("\"{0}\";\r\n", hash);
            //        else
            //        {
            //            foreach (string parentHash in parentHashes)
            //            {
            //                dotData.AppendFormat("\"{0}\" -> \"{1}\";\r\n", parentHash, hash);
            //            }
            //        }


            //    }

            //    foreach (BranchInfo b in repo.Branches)
            //    {

            //    }

            //    foreach (TagInfo t in repo.Tags)
            //    {
            //        GitObject target = t.PeeledTarget;

            //    }


            //}


            dotData.AppendLine("}");

            string tempPath = Path.GetTempFileName();
            File.WriteAllText(tempPath, dotData.ToString());

            ProcessStartInfo dotPsi = new ProcessStartInfo();
            dotPsi.FileName = _settings.DotExePath;
            dotPsi.Arguments = string.Format("-T{0} {1} -o\"{2}\"", _settings.TargetFormat, tempPath,
                _settings.TargetFilePath);
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