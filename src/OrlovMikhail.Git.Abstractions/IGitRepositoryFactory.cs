namespace OrlovMikhail.Git
{
    public interface IGitRepositoryFactory
    {
        IGitRepository Open(string path);
    }
}