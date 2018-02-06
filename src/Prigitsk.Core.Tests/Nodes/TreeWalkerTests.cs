using System.Linq;
using Moq;
using Prigitsk.Core.Nodes;
using Xunit;

namespace Prigitsk.Core.Tests.Nodes
{
    public class TreeWalkerTests
    {
        [Fact]
        public void GivenRhombus_ThenReturnsAllItemsOnce()
        {
            /*      b
             *     / \
             *    a   [d]
             *     \ /
             *      c
             */

            var a = Mock.Of<INode>();
            var b = Mock.Of<INode>(n => n.Parents == new[] {a});
            var c = Mock.Of<INode>(n => n.Parents == new[] {a});
            var d = Mock.Of<INode>(n => n.Parents == new[] {b, c});

            ITreeWalker tw = new TreeWalker();

            INode[] allNodes = tw.EnumerateAllParentsBreadthFirst(d, minimum: null).ToArray();
            Assert.Equal(new []{b, c, a}, allNodes);
        }
    }
}