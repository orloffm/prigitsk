namespace Prigitsk.Core.Git
{
    public interface IGitRepositoryFactory
    {
        IGitRepository Open(string path);
    }
}