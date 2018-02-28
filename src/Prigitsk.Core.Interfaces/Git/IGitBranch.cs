namespace Prigitsk.Core.Git
{
    public interface IGitBranch : IGitRef
    {
        bool IsRemote { get; }
    }
}