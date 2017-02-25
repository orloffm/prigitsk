namespace OrlovMikhail.GitTools.Loading.Client.Repository
{
    public interface IRepositoryDataBuilder
    {
        void AddCommit(string hash, string[] parentHashes);
        void AddCommitDescription(string hash, string description);
        void AddRemoteBranch(string friendlyName, string sourceHash);
        void AddTag(string friendlyName, string sourceHash);

        IRepositoryState Build();
    }
}