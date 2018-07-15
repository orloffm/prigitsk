using System.Drawing;

namespace OrlovMikhail.GraphViz.Writing
{
    public static class ColorAttributeExtensions
    {
        public static IAttrSet Color(this IAttrSet attrSet, IGraphVizColor value)
        {
            ColorAttribute a = new ColorAttribute(value);
            return attrSet.Add(a);
        }

        public static IAttrSet Color(this IAttrSet attrSet, Color value)
        {
            return attrSet.Color(GraphVizColor.FromRgb(value));
        }

        public static IAttrSet Color(this IAttrSet attrSet, string hexString)
        {
            return attrSet.Color(GraphVizColor.FromHex(hexString));
        }
    }
}