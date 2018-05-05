using System;
using System.Collections.Generic;

namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class Attribute<T> : IAttribute
    {
        public string StringValue => ValueToString();

        protected abstract T Value { get; }

        public bool Equals(IAttribute other)
        {
            var otherTyped = other as Attribute<T>;
            if (otherTyped == null)
            {
                return false;
            }

            return String.Equals(this.StringValue, otherTyped.StringValue);
        }

        protected abstract string ValueToString();
    }
}