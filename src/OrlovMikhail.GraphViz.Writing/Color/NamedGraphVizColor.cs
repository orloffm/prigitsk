namespace OrlovMikhail.GraphViz.Writing
{
    public sealed class NamedGraphVizColor
        : GraphVizColor
    {
        internal NamedGraphVizColor(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToGraphVizColorString()
        {
            return Name;
        }
    }
}