using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.RepoData;
using Prigitsk.Core.Tests.Helpers;
using Xunit;

namespace Prigitsk.Core.Tests.RepoData
{
    public class CommitsDataTests
    {
        [Fact]
        public void GivenSetOfCommits_ThenEnumeratesThemUp()
        {
            ICommit a = EH.MockCommit("a");
            ICommit b = EH.MockCommit("b", a);
            ICommit c = EH.MockCommit("c", b);
            ICommit d = EH.MockCommit("d", c);

            CommitsData cd = new CommitsData(new[] {a, b, c, d});

            // Act.
            ICommit[] commits = cd.EnumerateUpTheHistoryFrom(d).Take(25).ToArray();

            // Verify.
            Assert.Equal(new[] {d, c, b, a}, commits);
        }
    }
}