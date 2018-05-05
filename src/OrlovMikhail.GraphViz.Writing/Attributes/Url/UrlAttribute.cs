namespace OrlovMikhail.GraphViz.Writing
{
    public class UrlAttribute : StringAttribute
    {
        public UrlAttribute(string value) : base(value)
        {
        }
        public override string Key => "URL";

    }
}