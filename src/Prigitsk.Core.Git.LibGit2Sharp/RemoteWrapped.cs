using LibGit2Sharp;

namespace Prigitsk.Core.Git.LibGit2Sharp
{
    public sealed class RemoteWrapped : IRemote
    {
        private readonly Remote _remote;

        private RemoteWrapped(Remote remote)
        {
            _remote = remote;
        }

        public string Name => _remote.Name;

        public string Url => _remote.Url;

        public static IRemote Create(Remote remote)
        {
            return new RemoteWrapped(remote);
        }
    }
}