namespace OrlovMikhail.GraphViz.Writing
{
    public static class WidthAttributeExtensions
    {
        public static IAttrSet Width(this IAttrSet attrSet, decimal value)
        {
            WidthAttribute a = new WidthAttribute(value);
            attrSet.Add(a);
            return attrSet;
        }
    }
}