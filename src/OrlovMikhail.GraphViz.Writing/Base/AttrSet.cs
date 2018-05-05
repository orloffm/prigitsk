using System;

namespace OrlovMikhail.GraphViz.Writing
{
    public class AttrSet : IAttrSet
    {
        public static IAttrSet Empty => new AttrSet();

        public void Add(IAttribute a)
        {
            throw new NotImplementedException();
        }
    }
}