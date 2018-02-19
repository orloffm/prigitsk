namespace Prigitsk.Core.RepoData
{
    /// <summary>
    ///     Offline information about Git repository required for application.
    /// </summary>
    public interface IRepositoryData
    {
        ICommitsData Commits { get; }
        IRemotesData Remotes { get; }
        IBranchesData Branches { get; }
        ITagsData Tags { get; }
    }
}