namespace OrlovMikhail.GraphViz.Writing
{
    public class ArrowHeadAttribute : EnumAttribute<ArrowType>
    {
        public ArrowHeadAttribute(ArrowType value) : base(value)
        {
        }

        public override string Key => "arrowHead";

        protected override string GetStringValueRaw()
        {
            return base.GetStringValueRaw().ToLower();
        }
    }
}