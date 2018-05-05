namespace OrlovMikhail.GraphViz.Writing
{
    public class ArrowHeadAttribute : EnumAttribute<ArrowType>
    {
        public ArrowHeadAttribute(ArrowType value) : base(value)
        {
        }

        protected override string ValueToString()
        {
            return base.ValueToString().ToLower();
        }
    }
}