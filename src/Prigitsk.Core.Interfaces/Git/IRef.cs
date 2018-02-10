namespace Prigitsk.Core.Git
{
    public interface IRef
    {
        ICommit Tip { get; }
        string FriendlyName { get; }
    }
}