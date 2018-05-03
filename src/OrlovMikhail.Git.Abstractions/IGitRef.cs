namespace OrlovMikhail.Git
{
    public interface IGitRef
    {
        string FriendlyName { get; }

        IGitCommit Tip { get; }
    }
}