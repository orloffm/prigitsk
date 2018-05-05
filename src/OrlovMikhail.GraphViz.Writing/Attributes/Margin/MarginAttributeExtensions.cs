namespace OrlovMikhail.GraphViz.Writing
{
    public static class MarginAttributeExtensions
    {
        public static IAttrSet Margin(this IAttrSet attrSet, double value)
        {
            MarginAttribute a = new MarginAttribute(value);
            attrSet.Add(a);
            return attrSet;
        }
    }
}