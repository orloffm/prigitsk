using System;
using System.Globalization;

namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class PointAttribute : Attribute<Tuple<decimal, decimal>>
    {
        protected PointAttribute(decimal x, decimal y) : base(Tuple.Create(x, y))
        {
        }

        protected PointAttribute(decimal value) : base(Tuple.Create(value, value))
        {
        }

        protected override string GetStringValueRaw()
        {
            decimal x = Value.Item1;
            decimal y = Value.Item2;

            string result = x.ToString(CultureInfo.InvariantCulture);
            if (x != y)
            {
                result += ", " + y.ToString(CultureInfo.InvariantCulture);
            }

            return result;
        }
    }
}