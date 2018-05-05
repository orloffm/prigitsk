using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class Attribute<T> : IAttribute
    {
        internal Attribute(T value)
        {
            Value = value;
        }

        public abstract string Key { get; }

        public string StringValue => GetStringValueRaw();

        protected T Value { get; }

        public bool Equals(IAttribute other)
        {
            if (!(other is Attribute<T> otherTyped))
            {
                return false;
            }

            return string.Equals(StringValue, otherTyped.StringValue);
        }

        protected virtual string GetStringValueRaw()
        {
            return Value.ToString();
        }
    }
}