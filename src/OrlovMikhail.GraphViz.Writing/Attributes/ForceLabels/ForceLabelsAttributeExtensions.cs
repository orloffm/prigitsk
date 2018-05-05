namespace OrlovMikhail.GraphViz.Writing
{
    public static class ForceLabelsAttributeExtensions
    {
        public static IAttrSet ForceLabels(this IAttrSet attrSet, bool value)
        {
            ForceLabelsAttribute a = new ForceLabelsAttribute(value);
            attrSet.Add(a);
            return attrSet;
        }
    }
}