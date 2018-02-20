using System;
using System.Collections.Generic;

namespace Prigitsk.Core.Nodes
{
    public interface ITreeWalker
    {
        IEnumerable<INode> EnumerateAllParentsBreadthFirst(INode source, DateTime? minimum = null);

        IEnumerable<INode> EnumerateFirstParentsUpTheTreeBranchAgnostic(INode source, bool includeSelf = false);
    }
}