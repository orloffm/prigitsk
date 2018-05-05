using System.Globalization;

namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class DoubleAttribute : Attribute<double>
    {
        protected DoubleAttribute(double value)
        {
            Value = value;
        }

        protected override double Value { get; }

        protected override string ValueToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}