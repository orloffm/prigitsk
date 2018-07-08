namespace OrlovMikhail.GraphViz.Writing
{
    public static class FontNameAttributeExtensions
    {
        /// <summary>
        ///     Font used for text.
        /// </summary>
        public static IAttrSet FontName(this IAttrSet attrSet, string value)
        {
            FontNameAttribute a = new FontNameAttribute(value);
            attrSet.Add(a);
            return attrSet;
        }
    }
}