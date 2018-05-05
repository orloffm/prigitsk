namespace OrlovMikhail.GraphViz.Writing
{
    public class WidthAttribute : DoubleAttribute
    {
        public WidthAttribute(decimal value) : base(value)
        {
        }
        public override string Key => "width";

    }
}