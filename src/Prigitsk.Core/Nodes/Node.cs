using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Nodes
{
    public class Node : IEquatable<Node>
    {
        public Node(string hash)
        {
            Parents = new OrderedSet<Node>();
            Children = new HashSet<Node>();
            Hash = hash;
            SetCaptions(null);
        }

        public OrderedSet<Node> Parents { get; }
        public HashSet<Node> Children { get; }
        public string Hash { get; }
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

        public void AddChild(Node immediateChild)
        {
            if (!Children.Contains(immediateChild))
            {
                Children.Add(immediateChild);
            }
        }

        public void AddParent(Node immediateParent)
        {
            if (!Parents.Contains(immediateParent))
            {
                Parents.Add(immediateParent);
            }
        }

        public IEnumerable<Node> EnumerateFirstParentsUpTheTreeBranchAgnostic(bool includeSelf = false)
        {
            if (includeSelf)
            {
                yield return this;
            }

            Node n = Parents.FirstOrDefault();
            while (n != null)
            {
                yield return n;
                n = n.Parents.FirstOrDefault();
            }
        }

        public override int GetHashCode()
        {
            return Hash?.GetHashCode() ?? 0;
        }

        public void RemoveItselfFromTheNodeGraph()
        {
            // Parents.
            foreach (Node parent in Parents)
            {
                // Remove node.
                parent.Children.Remove(this);

                // Set its children to this parent.
                foreach (Node immediateChild in Children)
                {
                    parent.AddChild(immediateChild);
                }
            }

            // Children.
            foreach (Node child in Children)
            {
                // Remove the node from parents.
                child.Parents.Remove(this);

                // Set its parents to this child.
                foreach (Node immediateParent in Parents)
                {
                    child.AddParent(immediateParent);
                }
            }

            Node firstChild = Children.FirstOrDefault();
            if (firstChild != null)
            {
                firstChild.Insertions += Insertions;
                firstChild.Deletions += Deletions;
                firstChild.SetAsSomethingWasMergedInto();
            }

            // Clear references.
            Parents.Clear();
            Children.Clear();
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