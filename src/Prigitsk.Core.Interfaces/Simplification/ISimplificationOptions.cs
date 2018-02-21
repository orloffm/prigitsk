namespace Prigitsk.Core.Simplification
{
    public interface ISimplificationOptions
    {

        bool RemoveOrphans { get; }

        bool RemoveOrphansEvenWithTags { get; }

        bool LeaveNodesAfterLastMerge { get; }

        bool AggressivelyRemoveFirstBranchNodes { get;}
    }
}