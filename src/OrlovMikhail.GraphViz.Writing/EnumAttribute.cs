using System;
using System.Globalization;

namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class EnumAttribute<T> : Attribute<T> where T : struct, IConvertible
    {
        protected EnumAttribute(T enumValue)
        {
            Value = enumValue;
        }

        protected override T Value { get; }

        protected override string ValueToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture).ToLower();
        }
    }
}