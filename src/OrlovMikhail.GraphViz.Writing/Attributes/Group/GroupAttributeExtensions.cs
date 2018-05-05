namespace OrlovMikhail.GraphViz.Writing
{
    public static class GroupAttributeExtensions
    {
        /// <summary>
        ///     If the end points of an edge belong to the same group,
        ///     i.e., have the same group attribute,
        ///     parameters are set to avoid crossings and keep the edges straight.
        /// </summary>
        public static IAttrSet Group(this IAttrSet attrSet, string value)
        {
            GroupAttribute a = new GroupAttribute(value);
            attrSet.Add(a);
            return attrSet;
        }
    }
}