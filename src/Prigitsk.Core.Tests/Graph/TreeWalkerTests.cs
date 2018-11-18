using System;
using System.Linq;
using Prigitsk.Core.Graph;
using Prigitsk.Core.Tests.StubEntities;
using Xunit;

namespace Prigitsk.Core.Tests.Graph
{
    public class TreeWalkerTests
    {
        internal class TreeWalkerTestData
        {
            /*        [d]
           *          /
           *         b  - won't go earlier than this
           *        /
           *       a    - earlier
           *      /   /
           *     /   e  - later
           *    /   /
           *   x    c    - earlier; later
           *    \     /
           *     \   g  - equal
           *      \ /
           *       f    - earlier
           */

            public TreeWalkerTestData()
            {
                DateTimeOffset refTime = new DateTimeOffset(2018, 11, 18, 12, 0, 0, TimeSpan.Zero);
                DateTimeOffset earlierTime = refTime.AddHours(-1);
                DateTimeOffset laterTime = refTime.AddHours(+1);

                X = CreateMock("X", earlierTime);
                A = CreateMock("A", earlierTime, X);
                B = CreateMock("B", refTime, A);
                C = CreateMock("C", laterTime);
                E = CreateMock("E", laterTime, C);
                F = CreateMock("F", earlierTime, X);
                G = CreateMock("G", refTime, F);
                D = CreateMock("D", laterTime, B, E, G);

            }

            public INode A { get; }

            public INode B { get; }

            public INode C { get; }

            public INode D { get; }

            public INode E { get; }

            public INode F { get; }

            public INode G { get; }

            public INode X { get; }

            private INode CreateMock(string hash, DateTimeOffset time, params INode[] parents)
            {
                return new NodeStub(new CommitStub(hash, time), parents.ToArray());
            }
        }

        [Fact]
        public void GivenRhombus_ThenReturnsAllItemsOnce()
        {
            TreeWalkerTestData data = new TreeWalkerTestData();

            ITreeWalker tw = new TreeWalker();

            INode[] allNodes = tw.EnumerateAllParentsBreadthFirst(data.D, null).ToArray();
            Assert.Equal(new[] {data.B, data.E, data.G, data.A, data.C, data.F, data.X}, allNodes);
        }

        [Fact]
        public void GivenTreeAndMinimumDate_ThenDoesNotGoBeyondIt()
        {
            TreeWalkerTestData data = new TreeWalkerTestData();

            ITreeWalker tw = new TreeWalker();

            INode[] allNodes = tw.EnumerateAllParentsBreadthFirst(data.D, data.B).ToArray();
            Assert.Equal(new[] {data.B, data.E, data.G, data.C}, allNodes);
        }
    }
}