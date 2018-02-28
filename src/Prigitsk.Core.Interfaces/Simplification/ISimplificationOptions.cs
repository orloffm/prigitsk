namespace Prigitsk.Core.Simplification
{
    public interface ISimplificationOptions
    {
        bool AggressivelyRemoveFirstBranchNodes { get; }

        bool LeaveNodesAfterLastMerge { get; }

        bool RemoveOrphans { get; }

        bool RemoveOrphansEvenWithTags { get; }
    }
}