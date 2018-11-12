using System;

namespace Prigitsk.Core.Entities
{
    /// <summary>
    ///     Represents a remote branch.
    /// </summary>
    public sealed class Branch : Pointer, IBranch, IEquatable<Branch>
    {
        public Branch(string name, IHash tip) : base(name, tip)
        {
            int indexOfSlash = name.IndexOf('/');
            RemoteName = name.Substring(0, Math.Max(0, indexOfSlash));
            Label = name.Substring(indexOfSlash + 1);
        }

        public override string Label { get; }

        public string RemoteName { get; }

        public static bool AreEqual(IBranch a, IBranch b)
        {
            return ReferenceEquals(a, b);
        }

        public bool Equals(IBranch other)
        {
            return AreEqual(this, other);
        }

        public bool Equals(Branch other)
        {
            return AreEqual(this, other);
        }

        public override bool Equals(object obj)
        {
            return AreEqual(this, obj as IBranch);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Label;
        }
    }
}