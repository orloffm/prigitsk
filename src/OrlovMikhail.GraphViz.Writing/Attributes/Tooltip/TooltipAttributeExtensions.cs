namespace OrlovMikhail.GraphViz.Writing
{
    public static class TooltipAttributeExtensions
    {
        /// <summary>
        ///     Tooltip annotation attached to the node or edge.
        /// </summary>
        public static IAttrSet Tooltip(this IAttrSet attrSet, string value)
        {
            TooltipAttribute a = new TooltipAttribute(value);
            return attrSet.Add(a);
        }
    }
}