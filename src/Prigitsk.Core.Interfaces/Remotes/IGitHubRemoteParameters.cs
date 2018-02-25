namespace Prigitsk.Core.Remotes
{
    public interface IGitHubRemoteParameters
    {
        string Repository { get; }

        string Server { get; }

        string User { get; }
    }
}