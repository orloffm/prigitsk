namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class ColorAttribute : Attribute<IGraphVizColor>
    {
        protected ColorAttribute(IGraphVizColor color)
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