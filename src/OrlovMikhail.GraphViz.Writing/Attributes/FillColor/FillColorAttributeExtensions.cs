namespace OrlovMikhail.GraphViz.Writing
{
    public static class FillColorAttributeExtensions
    {
        public static IAttrSet FillColor(this IAttrSet attrSet, IGraphVizColor value)
        {
            FillColorAttribute a = new FillColorAttribute(value);
            attrSet.Add(a);
            return attrSet;
        }
    }
}