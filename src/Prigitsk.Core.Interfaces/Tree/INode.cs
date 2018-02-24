using System.Collections.Generic;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Tree
{
    public interface INode
    {
        IEnumerable<ICommit> AbsorbedCommits { get; }

        ISet<INode> Children { get; }

        ICommit Commit { get; set; }

        IOrderedSet<INode> Parents { get; }

        void AddAbsorbedCommit(ICommit commit);

        bool Equals(INode other);
    }
}