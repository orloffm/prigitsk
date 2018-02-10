using System;

namespace Prigitsk.Core.Nodes.Loading
{
    /// <summary>
    /// Collects data about repository and provides an immutable result.
    /// </summary>
    public interface IRepositoryDataBuilder
    {
        IRepositoryData Build();
        void AddCommit(string sha, string[] parentShas, DateTimeOffset committerWhen);
        void AddRemote(string remoteName, string rUrl);
        void AddBranch(string branchName, string tipSha);
        void AddTag(string tagName, string tipSha);
    }
}