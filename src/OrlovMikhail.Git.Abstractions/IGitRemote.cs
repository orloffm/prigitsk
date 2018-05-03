namespace OrlovMikhail.Git
{
    public interface IGitRemote
    {
        string Name { get; }

        string Url { get; }
    }
}