namespace OrlovMikhail.GraphViz.Writing
{
    public static class StyleAttributeExtensions
    {
        public static IAttrSet Style(this IAttrSet attrSet, Style value)
        {
            StyleAttribute a = new StyleAttribute(value);
            attrSet.Add(a);
            return attrSet;
        }
    }
}