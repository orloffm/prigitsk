using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using ConGitWriter.Helpers;
using OrlovMikhail.GitTools.Helpers;
using OrlovMikhail.GitTools.Loading.Client.Common;
using OrlovMikhail.GitTools.Loading.Client.Repository;
using OrlovMikhail.GitTools.Processing;
using OrlovMikhail.GitTools.Structure;

namespace ConGitWriter
{
    public class Worker : IWorker
    {
        private readonly IConGitWriterSettingsWrapper _settings;
        private readonly ISettingsHelper _settingsHelper;
        private readonly IProcessRunner _processRunner;
        private readonly IBranchingStrategy _branchingStrategy;
        private readonly IRepositoryProcessor _repositoryProcessor;
        private readonly IConsoleArgumentsHelper _consoleHelper;
        private readonly IGitClientFactory _gitClientFactory;
        private const string RepositoryPathArgumentName = "repository";
        private const string DotExeArgumentName = "dot";
        private const string GitExeArgumentName = "git";
        private const string TargetFileArgumentName = "target";
        private const string TargetDotFormatArgumentName = "format";

        public Worker(IConGitWriterSettingsWrapper settings,
            ISettingsHelper settingsHelper,
            IProcessRunner processRunner,
                        IBranchingStrategy branchingStrategy,
            IRepositoryProcessor repositoryProcessor,
            IConsoleArgumentsHelper consoleHelper,
            IGitClientFactory gitClientFactory)
        {
            _settings = settings;
            _settingsHelper = settingsHelper;
            _processRunner = processRunner;
            _branchingStrategy = branchingStrategy;
            _repositoryProcessor = repositoryProcessor;
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

            // Load the state.
            IRepositoryState state;
            using (IGitClient client = _gitClientFactory.CreateClient(_settings.RepositoryDirectory, _settings.GitExePath))
            {
                client.Init();
                state = client.Load();
            }

            RepositoryProcessingOptions options = RepositoryProcessingOptions.Default;
            IProcessedRepository processed = _repositoryProcessor.Process(state, options);

            StringBuilder dotData = new StringBuilder();
            dotData.AppendLine(@"strict digraph g
{
rankdir=""LR"";

");

            foreach (CommitInfo c in state.CommitInfos)
            {
                if (c.Parents.Length == 0)
                {
                    dotData.AppendFormat("\"{0}\";\r\n", c.Hash);
                }
                else
                {
                    foreach (CommitInfo p in c.Parents)
                    {
                        dotData.AppendFormat("\"{0}\" -> \"{1}\";\r\n", p.Hash, c.Hash);
                    }
                }
            }

            dotData.AppendLine("}");

            // Save generated Dot file.
            string tempPath = Path.GetTempFileName();
            File.WriteAllText(tempPath, dotData.ToString());

            // Run Dot to create the output file.
            string arguments = string.Format("-T{0} {1} -o\"{2}\"", _settings.TargetFormat, tempPath,
                _settings.TargetFilePath);
            _processRunner.Run(_settings.DotExePath, arguments);
        }
    }

}