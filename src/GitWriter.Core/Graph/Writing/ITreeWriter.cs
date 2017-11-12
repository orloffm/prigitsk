using GitWriter.Core.Graph.Strategy;
using GitWriter.Core.Nodes;

namespace GitWriter.Core.Graph.Writing
{
    public interface ITreeWriter
    {
        string GenerateGraph(IAssumedGraph graph, IBranchingStrategy branchingStrategy);
    }
}