namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class IntAttribute : Attribute<int>
    {
        protected IntAttribute(int value)
        {
            Value = value;
        }

        protected override int Value { get; }

        protected override string ValueToString()
        {
            return Value.ToString();
        }
    }
}