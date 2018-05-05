namespace OrlovMikhail.GraphViz.Writing
{
    public class RankAttribute : EnumAttribute<RankType>
    {
        public RankAttribute(RankType value) : base(value)
        {
        }

    }
}