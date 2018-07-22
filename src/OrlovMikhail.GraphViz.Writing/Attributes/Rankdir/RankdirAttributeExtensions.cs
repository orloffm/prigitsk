namespace OrlovMikhail.GraphViz.Writing
{
    public static class RankdirAttributeExtensions
    {
        public static IAttrSet Rankdir(this IAttrSet attrSet, Rankdir value)
        {
            RankdirAttribute a = new RankdirAttribute(value);
            return attrSet.Add(a);
        }
    }
}