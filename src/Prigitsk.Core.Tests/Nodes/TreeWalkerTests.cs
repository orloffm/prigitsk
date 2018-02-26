using System.Linq;
using Moq;
using Prigitsk.Core.Tree;

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

            INode a = Mock.Of<INode>();
            INode b = Mock.Of<INode>(n => n.Parents == new[] {a});
            INode c = Mock.Of<INode>(n => n.Parents == new[] {a});
            INode d = Mock.Of<INode>(n => n.Parents == new[] {b, c});

            ITreeWalker tw = new TreeWalker();

            INode[] allNodes = tw.EnumerateAllParentsBreadthFirst(d, null).ToArray();
            Assert.Equal(new[] {b, c, a}, allNodes);
        }

        [Fact]
        public void GivenTreeAndMinimumDate_ThenDoesNotGoBeyondIt()
        {
        }
    }
}