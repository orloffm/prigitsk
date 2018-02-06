using System;
using System.Collections.Generic;

namespace Prigitsk.Core.Nodes
{
    public interface ITreeWalker
    {
        IEnumerable<INode> EnumerateFirstParentsUpTheTreeBranchAgnostic(INode source, bool includeSelf = false);

        IEnumerable<INode> EnumerateAllParentsBreadthFirst(INode source, DateTime? minimum = null);
    }
}
