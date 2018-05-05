namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class BooleanAttribute : Attribute<bool>
    {
        protected BooleanAttribute(bool value) : base(value)
        {
        }

        protected override string GetStringValueRaw()
        {
            return Value ? "true" : "false";
        }
    }
}