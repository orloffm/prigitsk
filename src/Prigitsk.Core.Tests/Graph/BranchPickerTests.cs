using System;
using System.Collections.Generic;
using Moq;
using Prigitsk.Core.Graph;
using Xunit;

namespace Prigitsk.Core.Tests.Graph
{
    public sealed class BranchPickerTests
    {
        private void RunTestCase(BranchPickerTestCase testCase)
        {
            // Arrange.
            var pickingOptionsMock = new Mock<IBranchPickingOptions>();
            pickingOptionsMock.SetupGet(z => z.IncludeBranchesRegices).Returns(testCase.IncludeRegices);
            pickingOptionsMock.SetupGet(z => z.ExcludeBranchesRegices).Returns(testCase.ExcludeRegices);

            var expectedPickedSet = new HashSet<string>(testCase.ExpectedPicked, StringComparer.OrdinalIgnoreCase);

            BranchPicker picker = new BranchPicker(pickingOptionsMock.Object);

            // Act.
            var pickedLabels = new List<string>();
            foreach (string input in testCase.Input)
            {
                if (picker.ShouldBePicked(input))
                {
                    pickedLabels.Add(input);
                }
            }

            // Verify.
            Assert.Equal(expectedPickedSet.Count, pickedLabels.Count);
            foreach (string pickedItem in pickedLabels)
            {
                Assert.Contains(pickedItem, expectedPickedSet);
            }
        }

        [Fact]
        public void GivenEmptyIncludeRule_ThenReturnsAll()
        {
            BranchPickerTestCase testCase = new BranchPickerTestCase();
            testCase.Input = new[] {"a", "b", "c"};
            testCase.ExpectedPicked = testCase.Input;
            testCase.IncludeRegices = new string[0];

            RunTestCase(testCase);
        }

        [Fact]
        public void GivenIncludeAndExcludeRules_ThenTreatsIncludeWithHigherPriority()
        {
            BranchPickerTestCase testCase = new BranchPickerTestCase();
            testCase.Input = new[] {"a", "b", "c"};
            testCase.ExpectedPicked = new[] {"b"};
            testCase.IncludeRegices = new[] {"b"};
            testCase.ExcludeRegices = new[] {"b", "c"};

            RunTestCase(testCase);
        }

        [Fact]
        public void GivenNoRules_ThenReturnsAllBranches()
        {
            BranchPickerTestCase testCase = new BranchPickerTestCase();
            testCase.Input = new[] {"a", "b", "c"};
            testCase.ExpectedPicked = testCase.Input;

            RunTestCase(testCase);
        }

        [Fact]
        public void GivenSimpleExcludeRule_ThenTreatsItAsRegexForPicking()
        {
            BranchPickerTestCase testCase = new BranchPickerTestCase();
            testCase.Input = new[] {"a", "b", "c"};
            testCase.ExpectedPicked = new[] {"b", "c"};
            testCase.ExcludeRegices = new[] {"a"};

            RunTestCase(testCase);
        }

        [Fact]
        public void GivenSimpleIncludeRule_ThenTreatsItAsRegexForPicking()
        {
            BranchPickerTestCase testCase = new BranchPickerTestCase();
            testCase.Input = new[] {"a", "b", "c"};
            testCase.ExpectedPicked = new[] {"b", "c"};
            testCase.IncludeRegices = new[] {"[bc]"};

            RunTestCase(testCase);
        }

        [Fact]
        public void GivenTwoExcludeRules_ThenTreatsThemCorrectly()
        {
            BranchPickerTestCase testCase = new BranchPickerTestCase();
            testCase.Input = new[] {"a", "b", "c"};
            testCase.ExpectedPicked = new[] {"c"};
            testCase.ExcludeRegices = new[] {"a", "[bd]"};

            RunTestCase(testCase);
        }
    }
}