namespace OrlovMikhail.GraphViz.Writing
{
    public static class HeightAttributeExtensions
    {
        public static IAttrSet Height(this IAttrSet attrSet, decimal value)
        {
            HeightAttribute a = new HeightAttribute(value);
            return attrSet.Add(a);
        }
    }
}