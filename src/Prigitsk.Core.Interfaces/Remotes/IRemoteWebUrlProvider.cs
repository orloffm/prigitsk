using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Remotes
{
    /// <summary>
    ///     Provides web URLs for the specified objects.
    /// </summary>
    public interface IRemoteWebUrlProvider
    {
        string GetBaseUrl();

        string GetBranchLink(IBranch branch);

        string GetCommitLink(ICommit commit);

        string GetCompareCommitsLink(ICommit commitA, ICommit commitB);

        string GetTagLink(ITag tag);
    }
}