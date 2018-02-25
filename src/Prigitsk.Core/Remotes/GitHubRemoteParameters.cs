namespace Prigitsk.Core.Remotes
{
    public sealed class GitHubRemoteParameters
        : IGitHubRemoteParameters
    {
        public GitHubRemoteParameters(string server, string user, string repository)
        {
            Server = server;
            User = user;
            Repository = repository;
        }

        public string Repository { get; }

        public string Server { get; }

        public string User { get; }
    }
}