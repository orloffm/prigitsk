using System;
using System.Collections.Generic;

namespace Prigitsk.Core.Nodes
{
    public interface INode
    {
        ICollection<INode> Parents { get; }
        ICollection<INode> Children { get; }
        string Hash { get; }

        DateTime Time { get; }
        bool SomethingWasMergedInto { get; }

        bool HasTagsOrNonLocalBranches { get; }

        // TODO: refactor out
        int Insertions { get; set; }
        // TODO: refactor out
        int Deletions { get; set; }
        // TODO: refactor out
        void SetAsSomethingWasMergedInto();
    }
}