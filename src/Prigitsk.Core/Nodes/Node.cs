using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Nodes
{
    public class Node : IEquatable<Node>, INode
    {
        public Node(string hash)
        {
            Parents = new OrderedSet<INode>();
            Children = new HashSet<INode>();
            Hash = hash;
            SetCaptions(null);
        }

        public GitRef[] GitRefs { get; private set; }
        public DateTime Time { get; set; }
        public int Insertions { get; set; }
        public int Deletions { get; set; }
        public bool SomethingWasMergedInto { get; private set; }

        public bool HasTagsOrNonLocalBranches
        {
            get { return GitRefs.Any(c => !c.IsLocalBranch); }
        }

        public bool Equals(Node other)
        {
            return AreEqual(this, other);
        }

        public ICollection<INode> Parents { get; }
        public ICollection<INode> Children { get; }
        public string Hash { get; }
        
        public override int GetHashCode()
        {
            return Hash?.GetHashCode() ?? 0;
        }

        public void SetAsSomethingWasMergedInto()
        {
            SomethingWasMergedInto = true;
        }

        public void SetCaptions(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                GitRefs = new GitRef[0];
                return;
            }

            source = source.Trim();
            source = source.TrimStart('(');
            source = source.TrimEnd(')');

            string[] captionStrings = source.Split(new[] {", "}, StringSplitOptions.RemoveEmptyEntries);
            GitRefs = captionStrings.Select(GitRef.FromGitLogRefName).ToArray();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Time.ToString("D"));
            sb.Append(": ");
            if (Parents.Count > 0)
            {
                bool parentAdded = false;
                foreach (Node parent in Parents)
                {
                    if (parentAdded)
                    {
                        sb.Append(", ");
                    }
                    else
                    {
                        parentAdded = true;
                    }

                    sb.Append(parent.Hash);
                }

                sb.Append(" -> ");
            }

            sb.Append("[" + Hash + "]");
            if (Children.Count > 0)
            {
                sb.Append(" -> ");
                bool childrenAdded = false;
                foreach (Node child in Children)
                {
                    if (childrenAdded)
                    {
                        sb.Append(", ");
                    }
                    else
                    {
                        childrenAdded = true;
                    }

                    sb.Append(child.Hash);
                }
            }

            return sb.ToString();
        }

        public static bool AreEqual(Node a, Node b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (ReferenceEquals(null, b))
            {
                return false;
            }

            if (ReferenceEquals(null, a))
            {
                return false;
            }

            return string.Equals(a.Hash, b.Hash);
        }

        public override bool Equals(object obj)
        {
            return AreEqual(this, (Node) obj);
        }
    }
}