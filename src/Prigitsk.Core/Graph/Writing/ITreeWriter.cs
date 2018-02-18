using Prigitsk.Core.Graph.Strategy;

namespace Prigitsk.Core.Graph.Writing
{
    public interface ITreeWriter
    {
        string GenerateGraph(IAssumedGraph graph, IBranchingStrategy branchingStrategy);
    }
}