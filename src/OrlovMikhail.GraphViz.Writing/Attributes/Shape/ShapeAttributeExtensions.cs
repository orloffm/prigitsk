namespace OrlovMikhail.GraphViz.Writing
{
    public static class ShapeAttributeExtensions
    {
        public static IAttrSet Shape(this IAttrSet attrSet, Shape value)
        {
            ShapeAttribute a = new ShapeAttribute(value);
            return attrSet.Add(a);
        }
    }
}