using LibGit2Sharp;

namespace Prigitsk.Core.Git.LibGit2Sharp
{
    public sealed class GitBranchWrapped : GitRefWrapped<Commit>, IGitBranch
    {
        private readonly Branch _branch;

        private GitBranchWrapped(Branch branch) : base(branch)
        {
            _branch = branch;
        }

        public bool IsRemote => _branch.IsRemote;

        public override IGitCommit Tip => GitCommitWrapped.Create(_branch.Tip);

        public string UpstreamCanonicalName => _branch.UpstreamBranchCanonicalName;

        public static IGitBranch Create(Branch arg)
        {
            return new GitBranchWrapped(arg);
        }
    }
}