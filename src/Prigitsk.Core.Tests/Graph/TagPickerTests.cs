using System;
using Moq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Graph;
using Prigitsk.Core.Tests.StubEntities;
using Xunit;

namespace Prigitsk.Core.Tests.Graph
{
    public sealed class TagPickerTests
    {
        private void Run(TagPickerTestCase testCase)
        {
            INode MockNode(string hash, DateTimeOffset? date)
            {
                ICommit commit = new CommitStub(hash, date ?? DateTimeOffset.MinValue);
                INode node = new NodeStub(commit);
                return node;
            }

            IBranch MockBranch(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return null;
                }

                return new Branch(name, null);
            }

            ITagPickingOptions tpo = TagPickingOptions.Set(
                testCase.Mode,
                testCase.TakeCount,
                testCase.IncludeOrphanedTags);
            TagPicker p = new TagPicker(tpo);

            ITag ta = new Tag(testCase.TagAName, null);
            ITag tb = new Tag(testCase.TagBName, null);
            ITag tc = new Tag(testCase.TagCName, null);

            INode na = MockNode("na", testCase.CommitADate);
            INode nb = MockNode("nb", testCase.CommitBDate);
            INode nc = MockNode("nc", testCase.CommitCDate);

            IBranch ba = MockBranch(testCase.BranchAName);
            IBranch bb = MockBranch(testCase.BranchBName);
            IBranch bc = MockBranch(testCase.BranchCName);

            var tagInfos = new[]
            {
                new TagInfo(ta, na, ba),
                new TagInfo(tb, nb, bb),
                new TagInfo(tc, nc, bc)
            };

            p.PreProcessAllTags(tagInfos);

            Assert.Equal(testCase.ExpectedTagAPicked, p.CheckIfTagShouldBePicked(ta));
            Assert.Equal(testCase.ExpectedTagBPicked, p.CheckIfTagShouldBePicked(tb));
            Assert.Equal(testCase.ExpectedTagCPicked, p.CheckIfTagShouldBePicked(tc));
        }

        private static TagPickerTestCase CreateIdealCase()
        {
            TagPickerTestCase testCase = new TagPickerTestCase
            {
                Mode = TagPickingMode.All,
                TakeCount = 3,
                TagAName = "v1.0",
                TagBName = "v1.5",
                TagCName = "v2.0",
                CommitADate = new DateTimeOffset(2001, 1, 1, 1, 2, 3, TimeSpan.Zero),
                CommitBDate = new DateTimeOffset(2002, 1, 1, 1, 2, 3, TimeSpan.Zero),
                CommitCDate = new DateTimeOffset(2003, 1, 1, 1, 2, 3, TimeSpan.Zero),
                BranchAName = "master",
                BranchBName = "master",
                BranchCName = "master",
                ExpectedTagAPicked = true,
                ExpectedTagBPicked = true,
                ExpectedTagCPicked = true
            };
            return testCase;
        }

        [Fact]
        public void GivenAllOption_ThenReturnsEverything()
        {
            TagPickerTestCase testCase = CreateIdealCase();
            testCase.TakeCount = 2;
            testCase.BranchAName = null;
            testCase.BranchBName = "a";
            testCase.CommitADate = null;

            Run(testCase);
        }

        [Fact]
        public void GivenCountOption_ThenReturnsAppropriateCount()
        {
            TagPickerTestCase testCase = CreateIdealCase();
            testCase.Mode = TagPickingMode.Latest;
            testCase.TakeCount = 3;

            Run(testCase);

            testCase.TakeCount = 2;
            testCase.ExpectedTagAPicked = false;

            Run(testCase);

            testCase.TakeCount = 1;
            testCase.ExpectedTagBPicked = false;

            Run(testCase);
        }

        [Fact]
        public void GivenCountOptionAndNullDates_ThenReturnsAppropriateCount()
        {
            TagPickerTestCase testCase = CreateIdealCase();
            testCase.Mode = TagPickingMode.Latest;
            testCase.TakeCount = 2;
            testCase.CommitBDate = null;
            testCase.ExpectedTagBPicked = false;

            Run(testCase);
        }

        [Fact]
        public void GivenMasterOption_ThenReturnsTagsOnMaster()
        {
            TagPickerTestCase testCase = CreateIdealCase();
            testCase.Mode = TagPickingMode.AllOnMaster;
            testCase.TakeCount = 3;
            testCase.BranchBName = null;
            testCase.BranchCName = "a";
            testCase.CommitADate = null;
            testCase.ExpectedTagBPicked = false;
            testCase.ExpectedTagCPicked = false;

            Run(testCase);
        }

        [Fact]
        public void GivenNoneOption_ThenReturnsNothing()
        {
            TagPickerTestCase testCase = CreateIdealCase();
            testCase.Mode = TagPickingMode.None;
            testCase.TakeCount = 2;
            testCase.BranchAName = null;
            testCase.BranchBName = "a";
            testCase.CommitADate = null;
            testCase.ExpectedTagAPicked = false;
            testCase.ExpectedTagBPicked = false;
            testCase.ExpectedTagCPicked = false;

            Run(testCase);
        }
    }
}