namespace OrlovMikhail.GraphViz.Writing
{
    public static class UrlAttributeExtensions
    {
        /// <summary>
        ///     If the end points of an edge belong to the same group,
        ///     i.e., have the same Url attribute,
        ///     parameters are set to avoid crossings and keep the edges straight.
        /// </summary>
        public static IAttrSet Url(this IAttrSet attrSet, string value)
        {
            UrlAttribute a = new UrlAttribute(value);
            attrSet.Add(a);
            return attrSet;
        }
    }
}