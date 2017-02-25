using System;
using System.IO;
using System.IO.Abstractions;
using OrlovMikhail.GitTools.Helpers;
using OrlovMikhail.GitTools.Loading.Client.Common;
using OrlovMikhail.GitTools.Loading.Client.Repository;

namespace OrlovMikhail.GitTools.Loading.Client.GitExe
{
    public class GitExeClient : IGitClient
    {
        private readonly IFileSystem _fileSystem;
        private readonly IProcessRunner _processRunner;
        private readonly IRepositoryDataBuilderFactory _builderFactory;
        private readonly string _repositoryPath;
        private readonly string _settingsGitExePath;

        private const string prefixFormat = "--git-dir=\"{0}\" ";
        private const string logCommand = "log --reflog --full-history --pretty=\"%h|%p|%d\"";
        private readonly string prefix;

        public GitExeClient(IFileSystem fileSystem, IProcessRunner processRunner,
            IRepositoryDataBuilderFactory builderFactory, string repositoryPath, string settingsGitExePath)
        {
            _fileSystem = fileSystem;
            _processRunner = processRunner;
            _builderFactory = builderFactory;
            _repositoryPath = repositoryPath;
            _settingsGitExePath = settingsGitExePath;

            prefix = string.Format(prefixFormat, _repositoryPath);
        }

        private string MakeArgumentString(string command)
        {
            return prefix + command;
        }

        public void Dispose()
        {
        }

        public void Init()
        {
            if (!_fileSystem.File.Exists(_settingsGitExePath))
            {
                string message = string.Format("Git executable not found at \"{0}\".", _settingsGitExePath);
                throw new FileNotFoundException(message);
            }


            if (!_fileSystem.Directory.Exists(_repositoryPath))
            {
                string message = string.Format("No Git repository found at location \"{0}\".", _repositoryPath);
                throw new FileNotFoundException(message);
            }
        }

        public IRepositoryState Load(GitClientLoadingOptions? options = null)
        {
            IRepositoryDataBuilder builder = _builderFactory.CreateBuilder();

            string arguments = MakeArgumentString(logCommand);

            string result = _processRunner.Run(_settingsGitExePath, arguments);

            string[] lines = result.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string[] parts = line.Split('|');

                string hash = parts[0];
                string[] parentHashes = parts[1].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                builder.AddCommit(hash, parentHashes);

                string tagString = parts[2];
                if (!string.IsNullOrWhiteSpace(tagString))
                {
                    string trimmedTagString = tagString.Trim().TrimStart('(').TrimEnd(')').Trim();
                    string[] refs = trimmedTagString.Split(new[] {", "}, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string gitref in refs)
                    {
                        const string originPrefix = @"origin/";
                        const string tagPrefix = @"tag: ";

                        if (gitref.StartsWith(originPrefix))
                        {
                            string originBranch = gitref.Substring(originPrefix.Length);
                            builder.AddRemoteBranch(originBranch, hash);
                        }
                        else if (gitref.StartsWith(tagPrefix))
                        {
                            string tag = gitref.Substring(tagPrefix.Length);
                            builder.AddTag(tag, hash);
                        }
                    }
                }
            }

            IRepositoryState state = builder.Build();
            return state;
        }
    }
}