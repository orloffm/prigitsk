using System.Collections.Generic;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Tree
{
    public interface INode
    {
        ISet<INode> Children { get; }

        IOrderedSet<ICommit> Commits { get; }

        IOrderedSet<INode> Parents { get; }

        bool Equals(INode other);
    }
}