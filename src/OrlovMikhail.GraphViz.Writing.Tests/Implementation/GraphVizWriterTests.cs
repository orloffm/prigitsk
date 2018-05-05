using System;
using Moq;
using Thinktecture.IO;
using Thinktecture.IO.Adapters;
using Xunit;

namespace OrlovMikhail.GraphViz.Writing.Tests
{
    public class GraphVizWriterTests
    {
        [Fact]
        public void GivenNodeWithTwoAttributes_ThenWritesThemCorrectly()
        {
            IAttrSet attributes = AttrSet.Empty.Width(1).Label("x");

            IStringWriter writer = new StringWriterAdapter();

            var dotHelperMock = new Mock<IDotHelper>();
            dotHelperMock.Setup(m => m.EscapeId(It.IsAny<string>())).Returns((string s) => s);
            dotHelperMock.Setup(m => m.GetRecordFromAttribute(It.IsAny<IAttribute>()))
                .Returns((IAttribute a) => $"{a.Key}={a.StringValue}");

            GraphVizWriter g = new GraphVizWriter(writer, dotHelperMock.Object);
            g.Node("z", attributes);

            string result = writer.GetStringBuilder().ToString();
            string expected = $"z [width=1, label=x];{Environment.NewLine}";

            Assert.Equal(expected, result);
        }
    }
}