namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class GraphVizColorAttribute : Attribute<IGraphVizColor>
    {
        protected GraphVizColorAttribute(IGraphVizColor color)
        {
            Value = color;
        }

        protected override IGraphVizColor Value { get; }

        protected override string ValueToString()
        {
            return Value.ToGraphVizColorString();
        }
    }
}