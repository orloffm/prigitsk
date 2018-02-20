namespace Prigitsk.Core.Simplification
{
    public interface ISimplificationOptions
    {
        /// <summary>
        ///     Do remove first node in the branch.
        /// </summary>
        bool AggressivelyRemoveFirstBranchNodes { get;  }

        /// <summary>
        ///     Leave nodes after last merge, at the end of the branch.
        /// </summary>
        bool LeaveNodesAfterLastMerge { get; }

        /// <summary>
        ///     Remove redundancies at all.
        /// </summary>
        bool PreventSimplification { get; }
    }
}