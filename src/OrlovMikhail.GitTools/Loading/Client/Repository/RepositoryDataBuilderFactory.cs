namespace OrlovMikhail.GitTools.Loading.Client.Repository
{
    public class RepositoryDataBuilderFactory : IRepositoryDataBuilderFactory
    {
        public IRepositoryDataBuilder CreateBuilder()
        {
            return new RepositoryDataBuilder();
        }
    }
}