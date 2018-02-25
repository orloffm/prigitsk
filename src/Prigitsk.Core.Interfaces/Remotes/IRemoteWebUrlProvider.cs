using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Remotes
{
    /// <summary>
    /// Provides web URLs for the specified objects.
    /// </summary>
    public interface IRemoteWebUrlProvider
    {
        string GetBranchLink(IBranch branch);

        string GetBaseUrl();
    }
}