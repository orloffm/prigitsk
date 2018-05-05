using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Graph
{
    public interface INode : ITreeish
    {
        IEnumerable<ICommit> AbsorbedParentCommits { get; }

        IEnumerable<INode> Children { get; }

        ICommit Commit { get; }

        IEnumerable<INode> Parents { get; }

        bool Equals(INode other);
    }
}