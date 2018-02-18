using LibGit2Sharp;

namespace Prigitsk.Core.Git.LibGit2Sharp
{
    public sealed class TagWrapped : RefWrapped<GitObject>, ITag
    {
        private readonly Tag _tag;

        private TagWrapped(Tag tag) : base(tag)
        {
            _tag = tag;
        }

        public override ICommit Tip => CommitWrapped.Create(_tag.Target as Commit);

        public static ITag Create(Tag arg)
        {
            return new TagWrapped(arg);
        }
    }
}