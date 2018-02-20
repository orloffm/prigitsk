using System;
using System.Text;

namespace Prigitsk.Core.Nodes
{
    [Obsolete]
    public class GitRef : IEquatable<GitRef>
    {
        private const string OriginPrefix = @"origin/";
        private const string TagPrefix = @"tag: ";

        private GitRef()
        {
            IsTag = false;
            IsCommit = true;
        }

        public bool IsCommit
        {
            get => !IsTag;
            set => IsTag = !value;
        }

        public bool IsLocalBranch => IsCommit && !IsOriginBranch;

        public bool IsOriginBranch { get; private set; }

        public bool IsTag { get; private set; }

        public string ObjectType { get; set; }

        public string Value { get; private set; }

        public static GitRef FromForEachRef(
            string refName,
            string objectType)
        {
            GitRef c = new GitRef();
            if (refName.StartsWith(OriginPrefix))
            {
                refName = refName.Substring(OriginPrefix.Length);
                c.IsOriginBranch = true;
            }

            if (objectType == "tag")
            {
                c.IsTag = true;
            }

            c.Value = refName;
            return c;
        }

        public static GitRef FromGitLogRefName(string refNameEx)
        {
            GitRef c = new GitRef();
            if (refNameEx.StartsWith(TagPrefix))
            {
                refNameEx = refNameEx.Substring(TagPrefix.Length);
                c.IsTag = true;
            }
            else if (refNameEx.StartsWith(OriginPrefix))
            {
                refNameEx = refNameEx.Substring(OriginPrefix.Length);
                c.IsOriginBranch = true;
            }

            c.Value = refNameEx;
            return c;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (IsTag)
            {
                sb.Append(TagPrefix);
            }

            sb.Append(Value);
            return sb.ToString();
        }

        #region equality

        public static bool AreEqual(
            GitRef a,
            GitRef b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }

            return string.Equals(a.Value, b.Value) &&
                   a.IsCommit == b.IsCommit &&
                   a.IsOriginBranch == b.IsOriginBranch;
        }

        public override bool Equals(object obj)
        {
            return AreEqual(this, obj as GitRef);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(
            GitRef a,
            GitRef b)
        {
            return AreEqual(a, b);
        }

        public static bool operator !=(
            GitRef a,
            GitRef b)
        {
            return !AreEqual(a, b);
        }

        public bool Equals(GitRef other)
        {
            return AreEqual(this, other);
        }

        #endregion
    }
}