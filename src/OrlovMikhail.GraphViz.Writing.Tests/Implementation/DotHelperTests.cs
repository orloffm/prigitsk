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
        public void GivenString_WhenCheckedQuoted_ThenReturnsProperValues(string input, bool isQuotedProperly)
        {
            DotHelper dh = new DotHelper();
            bool result = dh.IsProperlyQuoted(input);
            Assert.Equal(isQuotedProperly, result);
        }

        [Fact]
        public void GivenStringAttribute_ThenReturnsProperRecordForIt()
        {
            LabelAttribute la = new LabelAttribute("b");
            DotHelper dh = new DotHelper();
            string record = dh.GetRecordFromAttribute(la);
            string expected = la.Key + "=b";

            Assert.Equal(expected, record);
        }
    }
}