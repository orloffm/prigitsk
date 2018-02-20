using System;

namespace Prigitsk.Core.Nodes
{
    [Obsolete]
    public class SimplificationOptions
    {
        /// <summary>Do remove first node in the branch.</summary>
        public bool AggressivelyRemoveFirstBranchNodes { get; set; }

        /// <summary>Leave nodes after last merge, at the end of the branch.</summary>
        public bool LeaveNodesAfterLastMerge { get; set; }

        /// <summary>Remove redundancies at all.</summary>
        public bool PreventSimplification { get; set; }
    }
}