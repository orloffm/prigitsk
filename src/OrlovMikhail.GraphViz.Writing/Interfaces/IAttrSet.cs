using System.Collections.Generic;

namespace OrlovMikhail.GraphViz.Writing
{
    public interface IAttrSet : IEnumerable<IAttribute>
    {
        void Add(IAttribute attr);
    }
}