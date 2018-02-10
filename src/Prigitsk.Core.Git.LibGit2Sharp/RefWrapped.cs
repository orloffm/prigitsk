using LibGit2Sharp;

namespace Prigitsk.Core.Git.LibGit2Sharp
{
    public abstract class RefWrapped<T> : IRef where T : GitObject
    {
        private readonly ReferenceWrapper<T> _gitRef;

        protected RefWrapped(ReferenceWrapper<T> gitref)
        {
            _gitRef = gitref;
        }

        public abstract ICommit Tip { get; }
        public string FriendlyName => _gitRef.FriendlyName;
    }
}