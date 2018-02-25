using System;

namespace Prigitsk.Core.Entities
{
    public class Tag
        : Pointer, ITag, IEquatable<Tag>
    {
        public Tag(string name, IHash tip) : base(name, tip)
        {
        }

        public static bool AreEqual(ITag a, ITag b)
        {
            return ReferenceEquals(a, b);
        }

        public bool Equals(ITag other)
        {
            return AreEqual(this, other);
        }

        public bool Equals(Tag other)
        {
            return AreEqual(this, other);
        }

        public override bool Equals(object obj)
        {
            return AreEqual(this, obj as ITag);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}