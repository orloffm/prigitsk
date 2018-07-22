namespace OrlovMikhail.GraphViz.Writing
{
    public static class ArrowHeadAttributeExtensions
    {
        public static IAttrSet ArrowHead(this IAttrSet attrSet, ArrowType value)
        {
            ArrowHeadAttribute a = new ArrowHeadAttribute(value);
            return attrSet.Add(a);
        }
    }
}