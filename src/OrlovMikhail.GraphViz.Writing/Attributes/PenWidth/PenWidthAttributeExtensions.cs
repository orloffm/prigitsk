namespace OrlovMikhail.GraphViz.Writing
{
    public static class PenWidthAttributeExtensions
    {
        public static IAttrSet PenWidth(this IAttrSet attrSet, decimal value)
        {
            PenWidthAttribute a = new PenWidthAttribute(value);
            attrSet.Add(a);
            return attrSet;
        }
    }
}