using System;

namespace Prigitsk.Core.RepoData
{
    /// <summary>
    ///     Collects data about repository and provides an immutable result.
    /// </summary>
    public interface IRepositoryDataBuilder
    {
        /// <summary>
        ///     Returns an immutable repository data set.
        /// </summary>
        IRepositoryData Build();

        void AddCommit(string sha, string[] parentShas, DateTimeOffset committerWhen);
        void AddRemote(string remoteName, string remoteUrl);
        void AddRemoteBranch(string branchName, string tipSha);
        void AddTag(string tagName, string tipSha);
    }
}