namespace OrlovMikhail.GraphViz.Writing
{
    public class PenWidthAttribute : DoubleAttribute
    {
        public PenWidthAttribute(decimal value) : base(value)
        {
        }
        public override string Key => "penWidth";
    }
}