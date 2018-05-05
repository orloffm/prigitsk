using System.Drawing;

namespace OrlovMikhail.GraphViz.Writing
{
    public static class FillColorAttributeExtensions
    {
        public static IAttrSet FillColor(this IAttrSet attrSet, IGraphVizColor value)
        {
            FillColorAttribute a = new FillColorAttribute(value);
            attrSet.Add(a);
            return attrSet;
        }

        public static IAttrSet FillColor(this IAttrSet attrSet, Color value)
        {
            return attrSet.FillColor(GraphVizColor.FromRgb(value));
        }

        public static IAttrSet FillColor(this IAttrSet attrSet, string hexString)
        {
            return attrSet.FillColor(GraphVizColor.FromHex(hexString));
        }
    }
}