namespace OrlovMikhail.GraphViz.Writing
{
    public class FixedSizeAttribute : BooleanAttribute
    {
        public FixedSizeAttribute(bool value) : base(value)
        {
        }
        public override string Key => "fixedSize";

    }
}