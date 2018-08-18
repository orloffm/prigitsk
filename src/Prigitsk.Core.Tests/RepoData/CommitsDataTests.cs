using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.RepoData;
using Prigitsk.Core.Tests.StubEntities;
using Xunit;

namespace Prigitsk.Core.Tests.RepoData
{
    public class CommitsDataTests
    {
        [Fact]
        public void GivenSetOfCommits_ThenEnumeratesThemUp()
        {
            ICommit a = new CommitStub("a");
            ICommit b = new CommitStub("b", a);
            ICommit c = new CommitStub("c", b);
            ICommit d = new CommitStub("d", c);

            CommitsData cd = new CommitsData(new[] {a, b, c, d});

            // Act.
            ICommit[] commits = cd.EnumerateUpTheHistoryFrom(d).Take(25).ToArray();

            // Verify.
            Assert.Equal(new[] {d, c, b, a}, commits);
        }
    }
}