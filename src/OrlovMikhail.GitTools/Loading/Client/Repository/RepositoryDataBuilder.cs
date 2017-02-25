namespace OrlovMikhail.GitTools.Loading.Client.Repository
{
    public class RepositoryDataBuilder : IRepositoryDataBuilder
    {
        public void AddCommit(string hash, string[] parentHashes, string description)
        {
           
        }

        public void AddRemoteBranch(string friendlyName, string sourceHash)
        {
           
        }

        public void AddTag(string friendlyName, string sourceHash)
        {
         
        }

        public IRepositoryData Build()
        {
            return null;
        }
    }
}