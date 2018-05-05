namespace OrlovMikhail.GraphViz.Writing
{
    public class FillColorAttribute : GraphVizColorAttribute
    {
        public FillColorAttribute(IGraphVizColor value) : base(value)
        {
        }

        public override string Key => "fillcolor";
    }
}