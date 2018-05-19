using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OrlovMikhail.GraphViz.Writing
{
    public sealed class AttrSet : IAttrSet
    {
        private readonly Dictionary<string, IAttribute> _set;

        public AttrSet()
        {
            _set = new Dictionary<string, IAttribute>(StringComparer.OrdinalIgnoreCase);
        }

        public static IAttrSet Empty => new AttrSet();

        public void Add(IAttribute a)
        {
            _set.Remove(a.Key);

            bool shouldAdd = a.StringValue != null;
            if (!shouldAdd)
            {
                return;
            }

            _set.Add(a.Key, a);
        }

        public IEnumerator<IAttribute> GetEnumerator()
        {
            return _set.Values.GetEnumerator();
        }

        public static bool NotNullOrEmpty(IAttrSet attrSet)
        {
            return attrSet != null && attrSet.Any();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}