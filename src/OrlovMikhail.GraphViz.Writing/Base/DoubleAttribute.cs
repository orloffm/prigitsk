using System.Globalization;

namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class DoubleAttribute : Attribute<decimal>
    {
        protected DoubleAttribute(decimal value) : base(value)
        {
        }

        protected override string GetStringValueRaw()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}