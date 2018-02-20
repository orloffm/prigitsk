using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace Prigitsk.Core.Git.LibGit2Sharp
{
    public class RepositoryWrapper : IRepository
    {
        private readonly Repository _repository;
        private bool _disposed;

        internal RepositoryWrapper(Repository repository)
        {
            _repository = repository;
        }

        public IEnumerable<IBranch> Branches
        {
            get
            {
                AssertNotDisposed();

                return _repository.Branches.Select(BranchWrapped.Create);
            }
        }

        public IEnumerable<ICommit> Commits
        {
            get
            {
                AssertNotDisposed();

                return _repository.Commits.Select(CommitWrapped.Create);
            }
        }

        public IEnumerable<IRemote> Remotes
        {
            get
            {
                AssertNotDisposed();

                return _repository.Network.Remotes.Select(RemoteWrapped.Create);
            }
        }

        public IEnumerable<ITag> TagsOnCommits
        {
            get
            {
                AssertNotDisposed();

                return _repository.Tags.Select(TagWrapped.Create).Where(t => t.Tip != null);
            }
        }

        private void AssertNotDisposed()
        {
            if (_disposed)
            {
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _repository.Dispose();
                _disposed = true;
            }
        }
    }
}