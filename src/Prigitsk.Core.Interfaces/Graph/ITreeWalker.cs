using System;
using System.Collections.Generic;

namespace Prigitsk.Core.Graph
{
    public interface ITreeWalker
    {
        IEnumerable<INode> EnumerateAllParentsBreadthFirst(INode source, INode dontGoEarlierThan = null);
    }
}