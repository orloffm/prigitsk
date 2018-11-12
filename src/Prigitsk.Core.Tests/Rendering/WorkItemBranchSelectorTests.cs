using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Strategy;
using Prigitsk.Core.Tests.StubEntities;
using Xunit;

namespace Prigitsk.Core.Tests.Rendering
{
    public class WorkItemBranchSelectorTests
    {
        private readonly string[] _commonList =
        {
            "master", "develop", "dev-ob", "dev-ob-1234", "dev-ob-1234-a", "dev-ob-POP-1234-c", "fake-1234",
            "-1234-develop"
        };

        private readonly string[] _expectedLesserOnes =
        {
            "dev-ob-1234", "dev-ob-1234-a", "dev-ob-POP-1234-c", "fake-1234",
            "-1234-develop"
        };

        private const string DefaultRegexString = @"-\d+";

        private void RunTestCase(
            IEnumerable<string> expectedLesserBranches,
            IEnumerable<string> branchesToCheck,
            string regexString)
        {
            var expectedSet = new HashSet<string>(expectedLesserBranches);
            IBranch[] branches = _commonList.Select(s => new BranchStub(s)).ToArray();
            WorkItemBranchSelector selector = new WorkItemBranchSelector();
            selector.PreProcessAllBranches(branches, new LesserBranchRegex(regexString));

            foreach (string branchName in branchesToCheck)
            {
                bool isLesser = selector.IsLesserBranch(new BranchStub(branchName));
                bool shouldBeMarked = expectedSet.Contains(branchName);

                Assert.Equal(shouldBeMarked, isLesser);
            }
        }

        [Fact]
        public void GivenArbitraryString_ThenReturnsNothing()
        {
            string[] expectedItems = { };
            string[] branchesToCheck = _commonList;
            string regexString = "AAA";

            RunTestCase(expectedItems, branchesToCheck, regexString);
        }

        [Fact]
        public void GivenDefaultString_ThenReturnsTheExpectedList()
        {
            string[] expectedItems = _expectedLesserOnes;
            string[] branchesToCheck = _commonList;
            string regexString = DefaultRegexString;

            RunTestCase(expectedItems, branchesToCheck, regexString);
        }

        [Fact]
        public void GivenNullString_ThenReturnsNothing()
        {
            string[] expectedItems = { };
            string[] branchesToCheck = _commonList;
            string regexString = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            RunTestCase(expectedItems, branchesToCheck, regexString);
        }
    }
}