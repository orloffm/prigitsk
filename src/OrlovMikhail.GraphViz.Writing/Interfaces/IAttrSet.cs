using System.Collections.Generic;

namespace OrlovMikhail.GraphViz.Writing
{
    public interface IAttrSet : IEnumerable<IAttribute>
    {
        IAttrSet Add(IAttribute attr);

        IAttrSet Add(IAttrSet attrSet);
    }
}