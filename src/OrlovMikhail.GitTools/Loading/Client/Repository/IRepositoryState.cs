namespace OrlovMikhail.GitTools.Loading.Client.Repository
{
    public interface IRepositoryState
    {
        CommitInfo[] CommitInfos { get; }
    }
}