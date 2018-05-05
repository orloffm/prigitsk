namespace OrlovMikhail.GraphViz.Writing
{
    public class FillColorAttribute : ColorAttribute
    {
        public FillColorAttribute(IGraphVizColor value) : base(value)
        {
        }
    }
}