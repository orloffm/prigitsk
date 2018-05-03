using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace OrlovMikhail.Git.LibGit2Sharp
{
    public class GitRepositoryWrapper : IGitRepository
    {
        private readonly Repository _repository;
        private bool _disposed;

        internal GitRepositoryWrapper(Repository repository)
        {
            _repository = repository;
        }

        public IEnumerable<IGitBranch> Branches
        {
            get
            {
                AssertNotDisposed();

                return _repository.Branches.Select(GitBranchWrapped.Create);
            }
        }

        public IEnumerable<IGitCommit> Commits
        {
            get
            {
                AssertNotDisposed();

                return _repository.Commits.QueryBy(new CommitFilter {IncludeReachableFrom = _repository.Refs})
                    .Select(GitCommitWrapped.Create);
            }
        }

        public IEnumerable<IGitRemote> Remotes
        {
            get
            {
                AssertNotDisposed();

                return _repository.Network.Remotes.Select(GitRemoteWrapped.Create);
            }
        }

        public IEnumerable<IGitTag> TagsOnCommits
        {
            get
            {
                AssertNotDisposed();

                return _repository.Tags.Select(GitTagWrapped.Create).Where(t => t.Tip != null);
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

        private void AssertNotDisposed()
        {
            if (_disposed)
            {
            }
        }
    }
}