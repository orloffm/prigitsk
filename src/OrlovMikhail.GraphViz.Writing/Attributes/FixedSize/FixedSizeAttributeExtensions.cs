namespace OrlovMikhail.GraphViz.Writing
{
    public static class FixedSizeAttributeExtensions
    {
        public static IAttrSet FixedSize(this IAttrSet attrSet, bool value)
        {
            FixedSizeAttribute a = new FixedSizeAttribute(value);
            return attrSet.Add(a);
        }
    }
}