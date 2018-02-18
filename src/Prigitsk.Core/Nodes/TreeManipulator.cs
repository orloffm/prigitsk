using System.Linq;

namespace Prigitsk.Core.Nodes
{
    public class TreeManipulator : ITreeManipulator
    {
        public void AddChild(INode source, INode immediateChild)
        {
            if (!source.Children.Contains(immediateChild))
            {
                source.Children.Add(immediateChild);
            }
        }

        public void AddParent(INode source, INode immediateParent)
        {
            if (!source.Parents.Contains(immediateParent))
            {
                source.Parents.Add(immediateParent);
            }
        }

        public void RemoveItselfFromTheNodeGraph(INode n)
        {
            // Parents.
            foreach (INode parent in n.Parents)
            {
                // Remove node.
                parent.Children.Remove(n);

                // Set its children to this parent.
                foreach (INode immediateChild in n.Children)
                {
                    AddChild(parent, immediateChild);
                }
            }

            // Children.
            foreach (INode child in n.Children)
            {
                // Remove the node from parents.
                child.Parents.Remove(n);

                // Set its parents to this child.
                foreach (INode immediateParent in n.Parents)
                {
                    AddParent(child, immediateParent);
                }
            }

            INode firstChild = n.Children.FirstOrDefault();
            if (firstChild != null)
            {
                firstChild.Insertions += n.Insertions;
                firstChild.Deletions += n.Deletions;
                firstChild.SetAsSomethingWasMergedInto();
            }

            // Clear references.
            n.Parents.Clear();
            n.Children.Clear();
        }
    }
}