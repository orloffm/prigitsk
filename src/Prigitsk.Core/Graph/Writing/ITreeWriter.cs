using Prigitsk.Core.Graph.Strategy;
using Prigitsk.Core.Nodes;

namespace Prigitsk.Core.Graph.Writing
{
    public interface ITreeWriter
    {
        string GenerateGraph(IAssumedGraph graph, IBranchingStrategy branchingStrategy);
    }
}