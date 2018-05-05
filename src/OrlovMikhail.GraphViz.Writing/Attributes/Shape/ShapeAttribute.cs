namespace OrlovMikhail.GraphViz.Writing
{
    public class ShapeAttribute : EnumAttribute<Shape>
    {
        public ShapeAttribute(Shape value) : base(value)
        {
        }
    }
}