namespace OrlovMikhail.GraphViz.Writing
{
    public static class MarginAttributeExtensions
    {
        public static IAttrSet Margin(this IAttrSet attrSet, decimal value)
        {
            return attrSet.Margin(value, value);
        }

        public static IAttrSet Margin(this IAttrSet attrSet, decimal horizontal, decimal vertical)
        {
            MarginAttribute a = new MarginAttribute(horizontal, vertical);
            attrSet.Add(a);
            return attrSet;
        }
    }
}