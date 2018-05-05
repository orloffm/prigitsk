using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace OrlovMikhail.GraphViz.Writing
{
    public sealed class RgbGraphVizColor : GraphVizColor
    {
        private const string HexRegexPattern =
            @"#(?<R>[ABCDEF\d]{2})(?<G>[ABCDEF\d]{2})(?<B>[ABCDEF\d]{2})(?<A>[ABCDEF\d]{2})?";

        private readonly string _hex;

        private RgbGraphVizColor(string hex)
        {
            _hex = hex;
        }

        public override string ToGraphVizColorString()
        {
            return _hex;
        }

        internal static RgbGraphVizColor CreateFromHex(string hex)
        {
            if (hex == null)
            {
                throw new ArgumentNullException();
            }

            hex = hex.Trim();

            if (!Regex.IsMatch(hex, HexRegexPattern))
            {
                throw new InvalidOperationException($"The '{hex}' value is not a proper hex color specification.");
            }

            return new RgbGraphVizColor(hex);
        }

        internal static RgbGraphVizColor CreateFromRgb(Color color)
        {
            string hex = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            if (color.A != byte.MaxValue)
            {
                hex += color.A.ToString("X2");
            }

            return new RgbGraphVizColor(hex);
        }
    }
}