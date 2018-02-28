using LibGit2Sharp;

namespace Prigitsk.Core.Git.LibGit2Sharp
{
    public sealed class GitTagWrapped : GitRefWrapped<GitObject>, IGitTag
    {
        private readonly Tag _tag;

        private GitTagWrapped(Tag tag) : base(tag)
        {
            _tag = tag;
        }

        public override IGitCommit Tip => GitCommitWrapped.Create(_tag.Target as Commit);

        public static IGitTag Create(Tag arg)
        {
            return new GitTagWrapped(arg);
        }
    }
}