namespace OrlovMikhail.GraphViz.Writing
{
    public static class RankAttributeExtensions
    {
        public static IAttrSet Rank(this IAttrSet attrSet, RankType value)
        {
            RankAttribute a = new RankAttribute(value);
            attrSet.Add(a);
            return attrSet;
        }
    }
}