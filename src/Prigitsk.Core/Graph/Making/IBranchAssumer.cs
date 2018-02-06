using System.Collections.Generic;
using Prigitsk.Core.Nodes;

namespace Prigitsk.Core.Graph.Making
{
    public interface IBranchAssumer
    {
        IAssumedGraph AssumeTheBranchGraph(IEnumerable<INode> allNodes);
    }
}