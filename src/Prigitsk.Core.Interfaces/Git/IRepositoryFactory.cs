namespace Prigitsk.Core.Git
{
    public interface IRepositoryFactory
    {
        IRepository Open(string path);
    }
}