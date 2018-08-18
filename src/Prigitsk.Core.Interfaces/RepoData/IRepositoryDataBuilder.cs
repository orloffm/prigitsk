using System;
using OrlovMikhail.Git;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.RepoData
{
    /// <summary>
    ///     Collects data about repository and provides an immutable result.
    /// </summary>
    public interface IRepositoryDataBuilder
    {
        void AddCommit(string sha, string[] parentShas, IGitSignature author, IGitSignature committer, string message);

        void AddRemote(string remoteName, string remoteUrl);

        void AddRemoteBranch(string branchName, string tipSha);

        void AddTag(string tagName, string tipSha);

        /// <summary>
        ///     Returns an immutable repository data set.
        /// </summary>
        IRepositoryData Build();
    }
}