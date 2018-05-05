namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class GraphVizColorAttribute : Attribute<IGraphVizColor>
    {
        protected GraphVizColorAttribute(IGraphVizColor color) : base(color)
        {
        }

        protected override string GetStringValueRaw()
        {
            return Value.ToGraphVizColorString();
        }
    }
}