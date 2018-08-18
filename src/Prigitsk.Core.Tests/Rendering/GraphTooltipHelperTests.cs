using System;
using Prigitsk.Core.Rendering;
using Prigitsk.Core.Tests.StubEntities;
using Xunit;

namespace Prigitsk.Core.Tests.Rendering
{
    public sealed class GraphTooltipHelperTests
    {
        private const string Treeish1 = "treeish1";
        private const string Treeish2 = "treeish2";
        private const string AuthorName = "aname";
        private const string AuthorEMail = "aemail";
        private readonly DateTimeOffset _authorWhen = new DateTimeOffset(2018, 08, 18, 1, 2, 3, TimeSpan.Zero);
        private const string CommitterName = "cname";
        private const string CommitterEMail = "cemail";
        private readonly DateTimeOffset _committerWhen = new DateTimeOffset(2018, 09, 18, 1, 2, 3, TimeSpan.Zero);
        private const string Message = "message";

        private CommitStub CreateCommitWithAuthor()
        {
            SignatureStub author = new SignatureStub {Name = AuthorName, EMail = AuthorEMail, When = _authorWhen};
            CommitStub commit =
                new CommitStub {Author = author, Committer = author, Treeish = Treeish1, Message = Message};
            return commit;
        }

        [Fact]
        public void GivenNodeWithDifferentCommitter_ThenReturnsExpectedText()
        {
            CommitStub commit = CreateCommitWithAuthor();
            SignatureStub committer =
                new SignatureStub {Name = CommitterName, EMail = CommitterEMail, When = _committerWhen};
            commit.Committer = committer;
            NodeStub node = new NodeStub(commit);

            GraphTooltipHelper helper = new GraphTooltipHelper();
            string result = helper.MakeNodeTooltip(node);

            Assert.Equal(
                $"{Treeish1} - {Message}" +
                $"{Environment.NewLine}{AuthorName} @ {_authorWhen}" +
                $"{Environment.NewLine}Committed by: {CommitterName} @ {_committerWhen}",
                result);
        }

        [Fact]
        public void GivenNodeWithSameCommitter_ThenReturnsExpectedText()
        {
            CommitStub commit = CreateCommitWithAuthor();
            NodeStub node = new NodeStub(commit);

            GraphTooltipHelper helper = new GraphTooltipHelper();
            string result = helper.MakeNodeTooltip(node);

            Assert.Equal(
                $"{Treeish1} - {Message}" +
                $"{Environment.NewLine}{AuthorName} @ {_authorWhen}",
                result);
        }

        [Fact]
        public void GivenTwoNodes_ThenReturnsExpectedTextForEdge()
        {
            NodeStub node1 = new NodeStub(new CommitStub {Treeish = Treeish1});
            NodeStub node2 = new NodeStub(new CommitStub {Treeish = Treeish2});

            GraphTooltipHelper helper = new GraphTooltipHelper();
            string result = helper.MakeEdgeTooltip(node1, node2);

            Assert.Equal($"{Treeish1} -> {Treeish2}", result);
        }
    }
}