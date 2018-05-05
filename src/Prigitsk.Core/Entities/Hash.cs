using System;

namespace Prigitsk.Core.Entities
{
    public sealed class Hash : IEquatable<Hash>, IHash
    {
        public const int DefaultCharsLength = 7;

        private Hash(string hash)
        {
            Value = hash;
        }

        public string Value { get; }

        public static bool AreEqual(IHash a, IHash b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(null, b))
            {
                return false;
            }

            return string.Equals(a.Value, b.Value, StringComparison.OrdinalIgnoreCase);
        }

        public static IHash Create(string s)
        {
            return new Hash(s);
        }

        public bool Equals(Hash other)
        {
            return AreEqual(this, other);
        }

        public bool Equals(IHash other)
        {
            return AreEqual(this, other);
        }

        public override bool Equals(object obj)
        {
            return AreEqual(this, obj as IHash);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public string ToShortString()
        {
            return Value.Length <= DefaultCharsLength ? Value : Value.Substring(0, DefaultCharsLength);
        }

        public override string ToString()
        {
            return ToShortString();
        }

        public string Treeish => ToShortString();
    }
}