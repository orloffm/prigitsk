namespace Prigitsk.Core.Git
{
    public interface IBranch : IRef
    {
        bool IsRemote { get; }
    }
}