namespace OrlovMikhail.GitTools.Common
{
    public class Color
    {
        private Color(string htmlColor)
        {
            HTMLColor = htmlColor;
        }

        public string HTMLColor { get; private set; }

        public static Color FromHTML(string htmlColor)
        {
            return new Color(htmlColor);
        }
    }
}