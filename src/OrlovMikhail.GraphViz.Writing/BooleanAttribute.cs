namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class BooleanAttribute : Attribute<bool>
    {
        protected BooleanAttribute(bool value)
        {
            Value = value;
        }

        protected override bool Value { get; }

        protected override string ValueToString()
        {
            return Value ? "true" : "false";
        }
    }
}