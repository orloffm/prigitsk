namespace OrlovMikhail.GraphViz.Writing
{
    public class FontNameAttribute : StringAttribute
    {
        public FontNameAttribute(string value) : base(value)
        {
        }

        public override string Key => "fontname";
    }
}