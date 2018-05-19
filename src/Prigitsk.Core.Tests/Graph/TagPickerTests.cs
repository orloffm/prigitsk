using System;
using Moq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Graph;
using Xunit;

namespace Prigitsk.Core.Tests.Graph
{
    public sealed class TagPickerTests
    {
        private void Run(TagPickerTestCase testCase)
        {
            ICommit MockCommit(DateTimeOffset? date)
            {
                ICommit commit = Mock.Of<ICommit>(c => c.CommittedWhen == date);
                return commit;
            }

            IBranch MockBranch(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return null;
                }

                return new Branch(name, null);
            }

            TagPicker p = new TagPicker(
                new TagPickingOptions
                {
                    LatestCount = testCase.TakeCount,
                    Mode = testCase.Mode
                });

            ITag ta = new Tag(testCase.TagAName, null);
            ITag tb = new Tag(testCase.TagBName, null);
            ITag tc = new Tag(testCase.TagCName, null);

            ICommit ca = MockCommit(testCase.CommitADate);
            ICommit cb = MockCommit(testCase.CommitBDate);
            ICommit cc = MockCommit(testCase.CommitCDate);

            IBranch ba = MockBranch(testCase.BranchAName);
            IBranch bb = MockBranch(testCase.BranchBName);
            IBranch bc = MockBranch(testCase.BranchCName);

            var tuples = new[]
            {
                Tuple.Create(ta, ca),
                Tuple.Create(tb, cb),
                Tuple.Create(tc, cc)
            };

            p.PreProcessAllTags(tuples);

            Assert.Equal(testCase.ExpectedTagAPicked, p.CheckIfTagShouldBePicked(ta, ba));
            Assert.Equal(testCase.ExpectedTagBPicked, p.CheckIfTagShouldBePicked(tb, bb));
            Assert.Equal(testCase.ExpectedTagCPicked, p.CheckIfTagShouldBePicked(tc, bc));
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