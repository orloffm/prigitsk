using LibGit2Sharp;

namespace Prigitsk.Core.Git.LibGit2Sharp
{
    public sealed class GitRepositoryFactory : IGitRepositoryFactory
    {
        public IGitRepository Open(string path)
        {
            Repository r = new Repository(path);
            GitRepositoryWrapper wrapper = new GitRepositoryWrapper(r);
            return wrapper;
        }
    }
}