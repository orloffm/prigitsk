namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class Attribute<T> : IAttribute
    {
        internal Attribute(T value)
        {
            Value = value;
        }

        public string StringValue => ValueToString();

        protected T Value { get; }

        public bool Equals(IAttribute other)
        {
            if (!(other is Attribute<T> otherTyped))
            {
                return false;
            }

            return string.Equals(StringValue, otherTyped.StringValue);
        }

        protected virtual string ValueToString()
        {
            return Value.ToString();
        }
    }
}