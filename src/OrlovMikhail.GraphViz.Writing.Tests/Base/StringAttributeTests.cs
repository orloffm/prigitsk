using Moq;
using Xunit;

namespace OrlovMikhail.GraphViz.Writing.Tests
{
    public class StringAttributeTests
    {
        [Fact]
        public void WhenCreatedWithNull_ThenReturnsNull()
        {
            var mock = new Mock<StringAttribute>(null);

            Assert.Null(mock.Object.StringValue);
        }
    }
}