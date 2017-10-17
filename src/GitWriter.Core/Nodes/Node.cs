using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitWriter.Core.Nodes
{
    public class Node
    {
        public Node(string hash)
        {
            Parents = new List<Node>();
            Children = new List<Node>();
            Hash = hash;
            SetCaptions(null);
        }

        public List<Node> Parents { get; }
        public List<Node> Children { get; }
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
            return Hash.GetHashCode();
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
                // Remove the node from parents, child.Parents.Remove(this);
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
                for (int index = 0; index < Parents.Count; index++)
                {
                    Node p = Parents[index];
                    if (index > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(p.Hash);
                }
                sb.Append(" -> ");
            }
            sb.Append("[" + Hash + "]");
            if (Children.Count > 0)
            {
                sb.Append(" -> ");
                for (int index = 0; index < Children.Count; index++)
                {
                    Node c = Children[index];
                    if (index > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(c.Hash);
                }
            }
            return sb.ToString();
        }
    }
}