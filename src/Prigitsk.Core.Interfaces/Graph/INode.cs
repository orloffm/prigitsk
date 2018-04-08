using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Graph
{
    public interface INode
    {
        IEnumerable<ICommit> AbsorbedChildCommits { get; }

        IEnumerable<INode> Children { get; }

        ICommit Commit { get; }

        IEnumerable<INode> Parents { get; }

        bool Equals(INode other);
    }
}