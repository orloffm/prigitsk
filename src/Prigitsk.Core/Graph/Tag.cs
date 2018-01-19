using System;
using Prigitsk.Core.Nodes;

namespace Prigitsk.Core.Graph
{
#pragma warning disable 660,661
    public sealed class Tag : Pointer, IEquatable<Tag>
#pragma warning restore 660,661
    {
        public Tag(string label, Node source)
            : base(label, source)
        {
        }

        public bool Equals(Tag other)
        {
            return AreEqual(this, other);
        }

        public static Tag FromCaption(GitRef c, Node node)
        {
            if (c.IsTag == false)
            {
                throw new InvalidOperationException();
            }

            return new Tag(c.Value, node);
        }

        public static bool operator ==(Tag a, Tag b)
        {
            return AreEqual(a, b);
        }

        public static bool operator !=(Tag a, Tag b)
        {
            return !AreEqual(a, b);
        }
    }
}