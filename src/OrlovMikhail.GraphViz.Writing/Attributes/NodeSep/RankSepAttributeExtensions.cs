namespace OrlovMikhail.GraphViz.Writing
{
    public static class RankSepAttributeExtensions
    {
        public static IAttrSet RankSep(this IAttrSet attrSet, double value)
        {
            RankSepAttribute a = new RankSepAttribute(value);
            attrSet.Add(a);
            return attrSet;
        }
    }
}