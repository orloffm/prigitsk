using System;

namespace OrlovMikhail.GitTools.Loading.Client.Repository
{
    public interface IRepositoryDataBuilder
    {
        void AddCommit(string hash, string[] parentHashes, string description);
        void AddRemoteBranch(string friendlyName, string sourceHash);
        void AddTag(string friendlyName, string sourceHash);

        IRepositoryData Build();
    }
}