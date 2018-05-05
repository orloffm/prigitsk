using Moq;
using Prigitsk.Core.Remotes;
using Xunit;

namespace Prigitsk.Core.Tests.Remotes
{
    public class RemoteWebUrlProviderFactoryTests
    {
        [Theory]
        [InlineData(@"git@github.com:gohugoio/hugo.git", false, true, "github.com", "gohugoio", "hugo")]
        [InlineData(@"https://github.com/gohugoio/hugo.git", false, true, "github.com", "gohugoio", "hugo")]
        [InlineData(@"https://github.somebank.com/gohugoio/hugo.git", false, false)]
        [InlineData(@"https://github.com/gohugoio/hugo", true, true, "github.com", "gohugoio", "hugo")]
        public void GivenAGitHubUrl_ThenReturnsGitHubProvider(
            string remoteUrl,
            bool forceGitHub,
            bool expectGitHub,
            string host = null,
            string user = null,
            string repository = null)
        {
            IGitHubRemoteParameters makerCalledWith = null;
            IGitHubRemoteWebUrlProvider providerMock = Mock.Of<IGitHubRemoteWebUrlProvider>();
            int makerCalledTimes = 0;

            IGitHubRemoteWebUrlProvider GitHubProviderMaker(IGitHubRemoteParameters gitHubRemoteParameters)
            {
                makerCalledWith = gitHubRemoteParameters;
                makerCalledTimes++;
                return providerMock;
            }

            IRemoteWebUrlProviderFactory f = new RemoteWebUrlProviderFactory(GitHubProviderMaker);

            // Act.
            IGitHubRemoteWebUrlProvider providerReturned =
                f.CreateUrlProvider(remoteUrl, forceGitHub) as IGitHubRemoteWebUrlProvider;

            // Verify.
            if (expectGitHub)
            {
                Assert.NotNull(providerReturned);
                Assert.Same(providerMock, providerReturned);
                Assert.Equal(1, makerCalledTimes);
                Assert.Equal(host, makerCalledWith.Server);
                Assert.Equal(user, makerCalledWith.User);
                Assert.Equal(repository, makerCalledWith.Repository);
            }
            else
            {
                Assert.Null(providerReturned);
                Assert.Equal(0, makerCalledTimes);
            }
        }
    }
}