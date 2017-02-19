namespace OrlovMikhail.GitTools.Loading
{
    public class RepositoryData : IRepositoryData
    {
        public void AddCommit(string mHash, string mDescription)
        {
            throw new System.NotImplementedException();
        }

        public void AddParentRelation(string parentHash, string mHash)
        {
            throw new System.NotImplementedException();
        }

        public void AddBranch(string bName, string bTargetCommitHash)
        {
            throw new System.NotImplementedException();
        }

        public void AddTag(string tName, string tTargetCommitHash)
        {
            throw new System.NotImplementedException();
        }
    }
}