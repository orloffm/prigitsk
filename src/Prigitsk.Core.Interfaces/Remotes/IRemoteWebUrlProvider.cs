using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Remotes
{
    /// <summary>
    ///     Provides web URLs for the specified objects.
    /// </summary>
    public interface IRemoteWebUrlProvider
    {
        string GetBaseUrl();

        /// <summary>
        /// Web link to a page representing the branch.
        /// </summary>
        string GetBranchLink(IBranch branch);

        /// <summary>
        /// Web link to a page allowing to manage the branch.
        /// </summary>
        string GetBranchMetaLink(IBranch branch);

        string GetCommitLink(ICommit commit);

        string GetCompareCommitsLink(ICommit commitA, ICommit commitB);

        string GetTagLink(ITag tag);
    }
}