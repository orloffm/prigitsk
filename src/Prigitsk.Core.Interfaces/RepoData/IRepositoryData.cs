namespace Prigitsk.Core.RepoData
{
    /// <summary>
    ///     Offline information about Git repository required for application.
    /// </summary>
    public interface IRepositoryData
    {
        IBranchesData Branches { get; }

        ICommitsData Commits { get; }

        IRemotesData Remotes { get; }

        ITagsData Tags { get; }
    }
}