using System;
using System.Collections.Generic;

namespace Prigitsk.Core.Nodes
{
    [Obsolete]
    public interface INode
    {
        ICollection<INode> Children { get; }

        // TODO: refactor out
        int Deletions { get; set; }

        string Hash { get; }

        bool HasTagsOrNonLocalBranches { get; }

        // TODO: refactor out
        int Insertions { get; set; }

        ICollection<INode> Parents { get; }

        bool SomethingWasMergedInto { get; }

        DateTime Time { get; }

        // TODO: refactor out
        void SetAsSomethingWasMergedInto();
    }
}