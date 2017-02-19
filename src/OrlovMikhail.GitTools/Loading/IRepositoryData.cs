namespace OrlovMikhail.GitTools.Loading
{
    public interface IRepositoryData
    {
        void AddCommit(string mHash, string mDescription);
        void AddParentRelation(string parentHash, string mHash);
        void AddBranch(string bName, string bTargetCommitHash);
        void AddTag(string tName, string tTargetCommitHash);
    }
}