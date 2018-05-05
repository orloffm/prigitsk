namespace OrlovMikhail.GraphViz.Writing
{
    public class RankdirAttribute : EnumAttribute<Rankdir>
    {
        public RankdirAttribute(Rankdir value) : base(value)
        {
        }

        public override string Key => "rankdir";
    }
}