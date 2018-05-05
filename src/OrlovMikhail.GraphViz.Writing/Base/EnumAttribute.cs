using System;

namespace OrlovMikhail.GraphViz.Writing
{
    public abstract class EnumAttribute<T> : Attribute<T> where T : struct, IConvertible
    {
        protected EnumAttribute(T enumValue) : base(enumValue)
        {
        }
    }
}