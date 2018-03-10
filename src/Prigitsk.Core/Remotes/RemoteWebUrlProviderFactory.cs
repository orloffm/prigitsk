using System;
using System.Text.RegularExpressions;

namespace Prigitsk.Core.Remotes
{
    public sealed class RemoteWebUrlProviderFactory : IRemoteWebUrlProviderFactory
    {
        private const string GitHubFqdn = "github.com";

        private const string RemoteRegex =
            @"(git@|https?://)(?<server>[^/:]+)[:/]{1}(?<user>[^/]+)/(?<repository>[^/\.]+)";

        /// <summary>
        ///     Factory that spawns web url providers for GitHub URLs.
        /// </summary>
        private readonly Func<IGitHubRemoteParameters, IGitHubRemoteWebUrlProvider> _gitHubProviderMaker;

        public RemoteWebUrlProviderFactory(
            Func<IGitHubRemoteParameters, IGitHubRemoteWebUrlProvider> gitHubProviderMaker)
        {
            _gitHubProviderMaker = gitHubProviderMaker;
        }

        public IRemoteWebUrlProvider CreateUrlProvider(string remoteUrl, bool forceGitHub = false)
        {
            Regex r = new Regex(RemoteRegex);
            Match m = r.Match(remoteUrl);
            if (!m.Success)
            {
                return null;
            }

            string server = m.Groups["server"].Value;

            bool isGitHub = string.Equals(server, GitHubFqdn, StringComparison.OrdinalIgnoreCase) || forceGitHub;
            if (!isGitHub)
            {
                return null;
            }

            string user = m.Groups["user"].Value;
            string repository = m.Groups["repository"].Value;

            GitHubRemoteParameters parameters = new GitHubRemoteParameters(server, user, repository);
            IGitHubRemoteWebUrlProvider gitHubProvider = _gitHubProviderMaker(parameters);
            return gitHubProvider;
        }
    }
}