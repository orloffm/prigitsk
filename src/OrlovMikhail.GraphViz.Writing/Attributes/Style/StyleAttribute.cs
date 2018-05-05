namespace OrlovMikhail.GraphViz.Writing
{
    public class StyleAttribute : EnumAttribute<Style>
    {
        public StyleAttribute(Style value) : base(value)
        {
        }

        protected override string GetStringValueRaw()
        {
            return Value.ToString().ToLower();
        }
        public override string Key => "style";

    }
}