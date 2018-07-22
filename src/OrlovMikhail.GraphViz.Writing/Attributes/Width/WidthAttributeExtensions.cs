namespace OrlovMikhail.GraphViz.Writing
{
    public static class WidthAttributeExtensions
    {
        public static IAttrSet Width(this IAttrSet attrSet, decimal value)
        {
            WidthAttribute a = new WidthAttribute(value);
            return attrSet.Add(a);
        }
    }
}