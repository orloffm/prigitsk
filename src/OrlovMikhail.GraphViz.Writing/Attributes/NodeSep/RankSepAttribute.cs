namespace OrlovMikhail.GraphViz.Writing
{
    public class RankSepAttribute : DoubleAttribute
    {
        public RankSepAttribute(decimal value) : base(value)
        {
        }

        public override string Key => "rankSep";
    }
}