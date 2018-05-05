namespace OrlovMikhail.GraphViz.Writing
{
    public class LabelAttribute : StringAttribute
    {
        public LabelAttribute(string value) : base(value)
        {
        }

        public override string Key => "label";
    }
}