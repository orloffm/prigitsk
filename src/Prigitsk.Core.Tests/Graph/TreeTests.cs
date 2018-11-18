using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Graph;
using Prigitsk.Core.Tests.StubEntities;
using Xunit;

namespace Prigitsk.Core.Tests.Graph
{
    public class TreeTests
    {
        [Fact]
        public void GivenNodeInBranch_WhenRemoved_ThenNotShownInBranch()
        {
            Tree t = new Tree();

            ICommit c1 = new CommitStub("h1");
            ICommit c2 = new CommitStub("h2", c1);
            ICommit c3 = new CommitStub("h3", c2);
            t.SetCommits(new[] {c1, c2, c3});

            IBranch b = new BranchStub("b", c3.Hash);

            t.AddBranch(b, new[] {c1.Hash, c2.Hash, c3.Hash});

            INode n2 = t.GetNode(c2.Hash);
            Assert.NotNull(n2);

            // Act.
            t.RemoveNode(n2);

            // Verify.
            INode[] nodesInBranch = t.EnumerateNodes(b).ToArray();
            Assert.Equal(2, nodesInBranch.Length);
            Assert.Equal(c1.Hash, nodesInBranch[0].Commit.Hash);
            Assert.Equal(c3.Hash, nodesInBranch[1].Commit.Hash);
        }
    }
}