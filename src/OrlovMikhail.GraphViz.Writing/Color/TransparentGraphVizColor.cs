namespace OrlovMikhail.GraphViz.Writing
{
    public sealed class TransparentGraphVizColor : GraphVizColor
    {
        internal TransparentGraphVizColor()
        {
        }

        public override string ToGraphVizColorString()
        {
            return "none";
        }
    }
}