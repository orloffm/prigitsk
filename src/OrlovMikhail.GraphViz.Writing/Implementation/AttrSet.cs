using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OrlovMikhail.GraphViz.Writing
{
    public sealed class AttrSet : IAttrSet
    {
        private readonly Dictionary<string, IAttribute> _set;

        private AttrSet(IDictionary<string, IAttribute> values = null)
        {
            if (values == null)
            {
                _set = new Dictionary<string, IAttribute>(StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                _set = new Dictionary<string, IAttribute>(values, StringComparer.OrdinalIgnoreCase);
            }
        }

        public static IAttrSet Empty => new AttrSet();

        public IAttrSet Add(IAttribute a)
        {
            bool shouldAdd = a.StringValue != null;
            if (!shouldAdd)
            {
                // Since no change
                return this;
            }

            AttrSet clonedSet = new AttrSet(_set);
            clonedSet._set[a.Key] = a;
            return clonedSet;
        }

        public IAttrSet Add(IAttrSet attrSet)
        {
            bool shouldAdd = attrSet.Any();
            if (!shouldAdd)
            {
                // Since no change
                return this;
            }

            AttrSet clonedSet = new AttrSet(_set);
            foreach (IAttribute item in attrSet)
            {
                clonedSet._set[item.Key] = item;
            }

            return clonedSet;
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