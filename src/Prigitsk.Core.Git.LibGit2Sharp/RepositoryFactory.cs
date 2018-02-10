using LibGit2Sharp;

namespace Prigitsk.Core.Git.LibGit2Sharp
{
    public sealed class RepositoryFactory : IRepositoryFactory
    {
        public IRepository Open(string path)
        {
            Repository r = new Repository(path);
            RepositoryWrapper wrapper = new RepositoryWrapper(r);
            return wrapper;
        }
    }
}