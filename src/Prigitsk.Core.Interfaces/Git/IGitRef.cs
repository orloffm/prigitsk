namespace Prigitsk.Core.Git
{
    public interface IGitRef
    {
        string FriendlyName { get; }

        IGitCommit Tip { get; }
    }
}