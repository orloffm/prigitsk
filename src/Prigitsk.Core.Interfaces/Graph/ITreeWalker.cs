using System;
using System.Collections.Generic;

namespace Prigitsk.Core.Graph
{
    public interface ITreeWalker
    {
        IEnumerable<INode> EnumerateAllParentsBreadthFirst(INode source, DateTimeOffset? minimum = null);

        IEnumerable<INode> EnumerateFirstParentsUpTheTreeBranchAgnostic(INode source, bool includeSelf = false);
    }
}