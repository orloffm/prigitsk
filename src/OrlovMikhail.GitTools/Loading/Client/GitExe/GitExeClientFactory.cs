using System.IO.Abstractions;
using OrlovMikhail.GitTools.Helpers;
using OrlovMikhail.GitTools.Loading.Client.Common;
using OrlovMikhail.GitTools.Loading.Client.Repository;

namespace OrlovMikhail.GitTools.Loading.Client.GitExe
{
    public class GitExeClientFactory : IGitClientFactory
    {
        private readonly IFileSystem _fileSystem;
        private readonly IProcessRunner _processRunner;
        private readonly IRepositoryDataBuilderFactory _builderFactory;

        public GitExeClientFactory(IFileSystem fileSystem, IProcessRunner processRunner,
            IRepositoryDataBuilderFactory builderFactory)
        {
            _fileSystem = fileSystem;
            _processRunner = processRunner;
            _builderFactory = builderFactory;
        }

        public IGitClient CreateClient(string repositoryPath, string settingsGitExePath)
        {
            return new GitExeClient(_fileSystem, _processRunner, _builderFactory, repositoryPath, settingsGitExePath);
        }
    }
}