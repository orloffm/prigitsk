using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OrlovMikhail.GraphViz.Writing
{
    public class AttrSet : IAttrSet
    {
        public static IAttrSet Empty => new AttrSet();

        public void Add(IAttribute a)
        {
            throw new NotImplementedException();
        }

        public static bool NotNullOrEmpty(IAttrSet attrSet)
        {
            return attrSet != null && attrSet.Any();
        }

        public IEnumerator<IAttribute> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}