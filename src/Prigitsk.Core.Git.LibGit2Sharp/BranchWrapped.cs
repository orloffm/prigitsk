using LibGit2Sharp;

namespace Prigitsk.Core.Git.LibGit2Sharp
{
    public sealed class BranchWrapped : RefWrapped<Commit>, IBranch
    {
        private readonly Branch _branch;

        private BranchWrapped(Branch branch) : base(branch)
        {
            _branch = branch;
        }

        public bool IsRemote => _branch.IsRemote;

        public override ICommit Tip => CommitWrapped.Create(_branch.Tip);

        public static IBranch Create(Branch arg)
        {
            return new BranchWrapped(arg);
        }
    }
}