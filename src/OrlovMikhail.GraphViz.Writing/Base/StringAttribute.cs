namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class StringAttribute : Attribute<string>
    {
        protected StringAttribute(string value) : base(value)
        {
        }

        protected override string GetStringValueRaw()
        {
            return Value;
        }
    }
}