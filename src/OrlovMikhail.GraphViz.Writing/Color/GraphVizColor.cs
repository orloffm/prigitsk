using System.Drawing;

namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class GraphVizColor : IGraphVizColor
    {
        public static IGraphVizColor None => new TransparentGraphVizColor();

        public static IGraphVizColor FromHex(string hex)
        {
            return RgbGraphVizColor.CreateFromHex(hex);
        }

        public static IGraphVizColor FromName(string name)
        {
            return new NamedGraphVizColor(name);
        }

        public static IGraphVizColor FromRgb(Color color)
        {
            if (color.IsNamedColor)
            {
                return new NamedGraphVizColor(color.Name.ToLower());
            }

            return RgbGraphVizColor.CreateFromRgb(color);
        }

        public abstract string ToGraphVizColorString();

        public override string ToString()
        {
            return ToGraphVizColorString();
        }
    }
}