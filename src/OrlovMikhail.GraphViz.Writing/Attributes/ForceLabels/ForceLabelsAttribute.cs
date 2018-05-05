namespace OrlovMikhail.GraphViz.Writing
{
    public class ForceLabelsAttribute : BooleanAttribute
    {
        public ForceLabelsAttribute(bool value) : base(value)
        {
        }

        public override string Key => "forcelabels";
    }
}