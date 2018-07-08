namespace OrlovMikhail.GraphViz.Writing
{
    public static class FontSizeAttributeExtensions
    {
        /// <summary>
        ///     Font size, in points, used for text.
        /// </summary>
        public static IAttrSet FontSize(this IAttrSet attrSet, decimal value)
        {
            FontSizeAttribute a = new FontSizeAttribute(value);
            attrSet.Add(a);
            return attrSet;
        }
    }
}