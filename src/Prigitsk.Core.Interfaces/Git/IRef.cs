namespace Prigitsk.Core.Git
{
    public interface IRef
    {
        string FriendlyName { get; }

        ICommit Tip { get; }
    }
}