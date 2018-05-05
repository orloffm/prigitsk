namespace OrlovMikhail.GraphViz.Writing
{
    public sealed class MarginAttribute : PointAttribute
    {
        public MarginAttribute(decimal value) : base(value)
        {
        }

        public MarginAttribute(decimal x, decimal y) : base(x, y)
        {
        }

        public override string Key => "margin";
    }
}