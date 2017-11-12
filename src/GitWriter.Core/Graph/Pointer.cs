using GitWriter.Core.Nodes;

namespace GitWriter.Core.Graph
{
    public abstract class Pointer
    {
        protected Pointer(string label, Node source)
        {
            Label = label;
            Source = source;
        }

        public Node Source { get; }
        public string Label { get; }

        public static bool AreEqual(Pointer a, Pointer b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(null, b))
            {
                return false;
            }

            if (a.GetType() != b.GetType())
            {
                return false;
            }

            return string.Equals(a.Label, b.Label);
        }

        public override bool Equals(object obj)
        {
            return AreEqual(this, obj as Pointer);
        }

        public override int GetHashCode()
        {
            return Label.GetHashCode();
        }
    }
}