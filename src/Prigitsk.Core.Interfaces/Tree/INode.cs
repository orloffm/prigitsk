using System;
using System.Collections.Generic;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Tree
{
    public interface INode
    {
        IOrderedCollection<INode> Parents { get; }
        ICollection<INode> Children { get; }
        IOrderedCollection<ICommit> Commits { get; }
        bool SomethingWasMergedInto { get; }

        ICollection<IBranch> Branches { get; }
        ICollection<ITag> Tags { get; }
    }
}