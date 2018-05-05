namespace OrlovMikhail.GraphViz.Writing
{
    public static class LenAttributeExtensions
    {
        /// <summary>
        ///     Preferred edge length, in inches. fdp, neato only.
        /// </summary>
        public static IAttrSet Len(this IAttrSet attrSet, decimal value)
        {
            LenAttribute a = new LenAttribute(value);
            attrSet.Add(a);
            return attrSet;
        }
    }
}