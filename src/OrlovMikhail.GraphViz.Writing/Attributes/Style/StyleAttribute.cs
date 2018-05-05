namespace OrlovMikhail.GraphViz.Writing
{
    public class StyleAttribute : EnumAttribute<Style>
    {
        public StyleAttribute(Style value) : base(value)
        {
        }

        protected override string ValueToString()
        {
            return Value.ToString().ToLower();
        }
    }
}