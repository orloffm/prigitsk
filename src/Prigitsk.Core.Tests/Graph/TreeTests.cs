using System.Linq;
using Moq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Graph;
using Xunit;

namespace Prigitsk.Core.Tests.Graph
{
    public class TreeTests
    {
        private IBranch CreateBranch(string s, IHash hash)
        {
            return Mock.Of<IBranch>(
                b => b.RemoteName == "origin"
                     && b.Tip == hash
                     && b.FullName == "origin/" + s
                     && b.Label == s);
        }

        private ICommit CreateCommitMock(string hashValue, params ICommit[] parents)
        {
            IHash hash = Mock.Of<IHash>(h => h.Value == hashValue);
            IHash[] parentHashes = parents.Select(c => c.Hash).ToArray();
            ICommit commit = Mock.Of<ICommit>(c => c.Hash == hash && c.Parents == parentHashes);
            return commit;
        }

        [Fact]
        public void GivenNodeInBranch_WhenRemoved_ThenNotShownInBranch()
        {
            Tree t = new Tree();

            ICommit c1 = CreateCommitMock("h1");
            t.AddCommit(c1);
            ICommit c2 = CreateCommitMock("h2", c1);
            t.AddCommit(c2);
            ICommit c3 = CreateCommitMock("h3", c2);
            t.AddCommit(c3);

            IBranch b = CreateBranch("b", c3.Hash);

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