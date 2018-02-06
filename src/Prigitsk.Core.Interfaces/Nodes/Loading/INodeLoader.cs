using System.Collections.Generic;

namespace Prigitsk.Core.Nodes.Loading
{
    public interface INodeLoader
    {
        IEnumerable<INode> GetNodesCollection();

        void LoadFrom(string gitSubDirectory, ExtractionOptions extractOptions);
    }
}