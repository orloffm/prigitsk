namespace OrlovMikhail.GraphViz.Writing
{
    public class ShapeAttribute : EnumAttribute<Shape>
    {
        public ShapeAttribute(Shape value) : base(value)
        {
        }

        protected override string GetStringValueRaw()
        {
            return Value.ToString().ToLower();
        }
        public override string Key => "shape";

    }
}