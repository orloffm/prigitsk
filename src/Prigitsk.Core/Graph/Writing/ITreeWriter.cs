using System;
using Prigitsk.Core.Graph.Strategy;

namespace Prigitsk.Core.Graph.Writing
{
    [Obsolete]
    public interface ITreeWriter
    {
        string GenerateGraph(IAssumedGraph graph, IBranchingStrategy branchingStrategy);
    }
}