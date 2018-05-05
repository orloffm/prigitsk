namespace OrlovMikhail.GraphViz.Writing
{
    public class ShapeAttribute : EnumAttribute<Shape>
    {
        public ShapeAttribute(Shape value) : base(value)
        {
        }

        public override string Key => "shape";

        protected override string GetStringValueRaw()
        {
            return Value.ToString().ToLower();
        }
    }
}