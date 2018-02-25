using Microsoft.Extensions.Logging;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Remotes
{
    public class GitHubRemoteWebUrlProvider : IRemoteWebUrlProvider
    {
        private readonly string _baseUrl;
        private readonly ILogger _log;

        public GitHubRemoteWebUrlProvider(IGitHubRemoteParameters parameters, ILogger log)
        {
            _baseUrl = string.Format(@"https://{0}/{1}/{2}", parameters.Server, parameters.User, parameters.Repository);
            _log = log;
        }

        public string GetBaseUrl()
        {
            return _baseUrl;
        }

        public string GetBranchLink(IBranch branch) => $"{_baseUrl}/tree/{branch.Label}";
    }
}