using LibGit2Sharp;

namespace Prigitsk.Core.Git.LibGit2Sharp
{
    public abstract class GitRefWrapped<T> : IGitRef where T : GitObject
    {
        private readonly ReferenceWrapper<T> _gitRef;

        protected GitRefWrapped(ReferenceWrapper<T> gitref)
        {
            _gitRef = gitref;
        }

        public string FriendlyName => _gitRef.FriendlyName;

        public abstract IGitCommit Tip { get; }
    }
}