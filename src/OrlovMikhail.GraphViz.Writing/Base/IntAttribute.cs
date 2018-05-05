namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class IntAttribute : Attribute<int>
    {
        protected IntAttribute(int value) : base(value)
        {
        }
    }
}