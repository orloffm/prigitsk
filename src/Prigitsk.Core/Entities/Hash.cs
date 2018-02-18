using System;
using System.Diagnostics;

namespace Prigitsk.Core.Entities
{
    [DebuggerDisplay("{this." + nameof(ToShortString) + "()}")]
    public sealed class Hash : IEquatable<Hash>, IHash
    {
        public const int DefaultCharsLength = 7;

        private Hash(string hash)
        {
            Value = hash;
        }

        public string ToShortString()
        {
            return Value.Length <= DefaultCharsLength ? Value : Value.Substring(0, DefaultCharsLength);
        }

        public bool Equals(Hash other)
        {
            return AreEqual(this, other);
        }

        public string Value { get; }

        public bool Equals(IHash other)
        {
            return AreEqual(this, other);
        }

        public override string ToString()
        {
            return Value;
        }

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

        public override bool Equals(object obj)
        {
            return AreEqual(this, obj as IHash);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static IHash Create(string s)
        {
            return new Hash(s);
        }
    }
}