using System;
using Prigitsk.Core.Graph;

namespace Prigitsk.Core.Nodes
{
    [Obsolete]
    public interface INodeCleaner
    {
        void CleanUpGraph(
            IAssumedGraph graph,
            SimplificationOptions options);
    }
}