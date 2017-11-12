using System;
using GitWriter.Core.Nodes;

namespace GitWriter.Core.Graph
{
#pragma warning disable 660,661
    public class OriginBranch : Pointer, IEquatable<OriginBranch>
#pragma warning restore 660,661
    {
        public OriginBranch(string label, Node source)
            : base(label, source)
        {
        }

        public bool Equals(OriginBranch other)
        {
            return AreEqual(this, other);
        }

        public static OriginBranch FromCaption(GitRef c, Node node)
        {
            if (c.IsOriginBranch == false)
            {
                throw new InvalidOperationException();
            }

            return new OriginBranch(c.Value, node);
        }

        public static bool operator ==(OriginBranch a, OriginBranch b)
        {
            return AreEqual(a, b);
        }

        public static bool operator !=(OriginBranch a, OriginBranch b)
        {
            return !AreEqual(a, b);
        }
    }
}