namespace OrlovMikhail.GraphViz.Writing
{
    public class HeightAttribute : DoubleAttribute
    {
        public HeightAttribute(decimal value) : base(value)
        {
        }
        public override string Key => "height";

    }
}