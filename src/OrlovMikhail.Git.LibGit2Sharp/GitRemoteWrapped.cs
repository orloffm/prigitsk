using LibGit2Sharp;

namespace OrlovMikhail.Git.LibGit2Sharp
{
    public sealed class GitRemoteWrapped : IGitRemote
    {
        private readonly Remote _remote;

        private GitRemoteWrapped(Remote remote)
        {
            _remote = remote;
        }

        public string Name => _remote.Name;

        public string Url => _remote.Url;

        public static IGitRemote Create(Remote remote)
        {
            return new GitRemoteWrapped(remote);
        }
    }
}