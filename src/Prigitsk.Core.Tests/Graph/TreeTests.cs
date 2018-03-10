﻿using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Graph;
using Prigitsk.Core.Tests.Helpers;
using Xunit;

namespace Prigitsk.Core.Tests.Graph
{
    public class TreeTests
    {
        [Fact]
        public void GivenNodeInBranch_WhenRemoved_ThenNotShownInBranch()
        {
            Tree t = new Tree();

            ICommit c1 = EH.MockCommit("h1");
            t.AddCommit(c1);
            ICommit c2 = EH.MockCommit("h2", c1);
            t.AddCommit(c2);
            ICommit c3 = EH.MockCommit("h3", c2);
            t.AddCommit(c3);

            IBranch b = EH.MockBranch("b", c3.Hash);

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