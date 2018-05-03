namespace OrlovMikhail.Git
{
    public interface IGitBranch : IGitRef
    {
        bool IsRemote { get; }

        string UpstreamCanonicalName { get; }
    }
}