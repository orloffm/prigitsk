namespace OrlovMikhail.GraphViz.Writing
{
    public static class RankSepAttributeExtensions
    {
        public static IAttrSet RankSep(this IAttrSet attrSet, decimal value)
        {
            RankSepAttribute a = new RankSepAttribute(value);
            return attrSet.Add(a);
        }
    }
}