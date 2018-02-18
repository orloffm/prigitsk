using Prigitsk.Core.Entities;

namespace Prigitsk.Core.RepoData
{
    /// <summary>
    ///     Offline information about Git repository required for application.
    /// </summary>
    public interface IRepositoryData
    {
        ICommit[] Commits { get; }
        IRemote[] Remotes { get; }
        IBranch[] Branches { get; }
        ITag[] Tags { get; }
    }
}