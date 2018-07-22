namespace OrlovMikhail.GraphViz.Writing
{
    public class FontSizeAttribute : DoubleAttribute
    {
        public FontSizeAttribute(decimal value) : base(value)
        {
        }

        public override string Key => "fontsize";
    }
}