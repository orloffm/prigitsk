namespace OrlovMikhail.GraphViz.Writing
{
    public class StyleAttribute : EnumAttribute<Style>
    {
        public StyleAttribute(Style value) : base(value)
        {
        }

        public override string Key => "style";

        protected override string GetStringValueRaw()
        {
            return Value.ToString().ToLower();
        }
    }
}