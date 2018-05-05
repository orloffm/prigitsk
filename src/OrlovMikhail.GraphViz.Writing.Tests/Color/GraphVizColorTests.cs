using System;
using Xunit;

namespace OrlovMikhail.GraphViz.Writing.Tests
{
    public class GraphVizColorTests
    {
        [Theory]
        [InlineData("#ababab")]
        [InlineData("#ABABABAB")]
        public void GivenProperHexData_ThenUsesItCorrectly(string source)
        {
            IGraphVizColor color = GraphVizColor.FromHex(source);
            string parsed = color.ToGraphVizColorString();
            Assert.Equal(source, parsed, StringComparer.OrdinalIgnoreCase);
        }
    }
}