using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Tree
{
    public interface INode
    {
        IEnumerable<ICommit> AbsorbedCommits { get; }

        IEnumerable<INode> Children { get; }

        ICommit Commit { get; }

        IEnumerable<INode> Parents { get; }

        bool Equals(INode other);
    }
}