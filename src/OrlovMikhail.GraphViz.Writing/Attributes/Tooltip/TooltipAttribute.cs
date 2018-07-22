namespace OrlovMikhail.GraphViz.Writing
{
    public class TooltipAttribute : StringAttribute
    {
        public TooltipAttribute(string value) : base(value)
        {
        }

        public override string Key => "tooltip";
    }
}