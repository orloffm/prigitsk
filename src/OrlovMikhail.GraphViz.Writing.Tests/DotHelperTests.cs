using Xunit;

namespace OrlovMikhail.GraphViz.Writing.Tests
{
    public class DotHelperTests
    {
        [Theory]
        [InlineData(@"a", false)]
        [InlineData(@"", false)]
        [InlineData("\"a\"", true)]
        [InlineData(@"""a\""", false)]
        [InlineData(@"""a\""""", true)]
        public void GivenString_WhenCheckedQuoted_ReturnsProperValues(string input, bool isQuotedProperly)
        {
            DotHelper dh = new DotHelper();
            bool result = dh.IsProperlyQuoted(input);
            Assert.Equal(isQuotedProperly, result);
        }
    }
}