namespace OrlovMikhail.GraphViz.Writing
{
    public class LenAttribute : DoubleAttribute
    {
        public LenAttribute(decimal value) : base(value)
        {
        }

        public override string Key => "len";
    }
}