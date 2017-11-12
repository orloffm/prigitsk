using System.Collections.Generic;
using GitWriter.Core.Nodes;

namespace GitWriter.Core.Graph.Making
{
    public interface IBranchAssumer
    {
        IAssumedGraph AssumeTheBranchGraph(IEnumerable<Node> allNodes);
    }
}