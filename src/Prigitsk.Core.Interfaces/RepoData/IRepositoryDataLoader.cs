namespace Prigitsk.Core.RepoData
{
    /// <summary>
    ///     Loads offline data from a Git repository.
    /// </summary>
    public interface IRepositoryDataLoader
    {
        IRepositoryData LoadFrom(string gitRepository);
    }
}