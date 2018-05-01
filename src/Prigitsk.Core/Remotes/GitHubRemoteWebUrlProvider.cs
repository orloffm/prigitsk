using Microsoft.Extensions.Logging;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Remotes
{
    public sealed class GitHubRemoteWebUrlProvider : IGitHubRemoteWebUrlProvider
    {
        private readonly string _baseUrl;
        private readonly ILogger _log;

        public GitHubRemoteWebUrlProvider(IGitHubRemoteParameters parameters, ILogger<GitHubRemoteWebUrlProvider> log)
        {
            _baseUrl = string.Format(@"https://{0}/{1}/{2}", parameters.Server, parameters.User, parameters.Repository);
            _log = log;
        }

        public string GetBaseUrl()
        {
            return _baseUrl;
        }

        public string GetBranchLink(IBranch branch)
        {
            return $"{_baseUrl}/tree/{branch.Label}";
        }

        public string GetCommitLink(ICommit commit)
        {
            return $"{_baseUrl}/commit/{commit.Hash.ToShortString()}";
        }

        public string GetCompareCommitsLink(ICommit nodeA, ICommit nodeB)
        {
            return $"{_baseUrl}/compare/{nodeA.Hash.ToShortString()}...{nodeB.Hash.ToShortString()}";
        }

        public string GetTagLink(ITag tag)
        {
            return $"{_baseUrl}/releases/tag/{tag.Label}";
        }
    }
}