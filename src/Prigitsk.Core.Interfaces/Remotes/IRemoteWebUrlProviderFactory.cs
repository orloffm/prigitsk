namespace Prigitsk.Core.Remotes
{
    /// <summary>
    ///     Creates providers of web links to repository objects by remotes.
    /// </summary>
    public interface IRemoteWebUrlProviderFactory
    {
        /// <summary>
        ///     Creates providers of web links to repository objects.
        /// </summary>
        /// <param name="remoteUrl">The remote to create a Url provider by.</param>
        /// <param name="forceGitHub">Whether to treat the path as a GitHub repository, despite the actual server name.</param>
        /// <returns>The Url provider or null if it cannot be created.</returns>
        IRemoteWebUrlProvider CreateUrlProvider(string remoteUrl, bool forceGitHub = false);
    }
}