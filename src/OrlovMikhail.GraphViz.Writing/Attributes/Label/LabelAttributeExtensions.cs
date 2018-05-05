namespace OrlovMikhail.GraphViz.Writing
{
    public static class LabelAttributeExtensions
    {
        /// <summary>
        ///     Text label attached to objects.
        /// </summary>
        public static IAttrSet Label(this IAttrSet attrSet, string value)
        {
            LabelAttribute a = new LabelAttribute(value);
            attrSet.Add(a);
            return attrSet;
        }
    }
}