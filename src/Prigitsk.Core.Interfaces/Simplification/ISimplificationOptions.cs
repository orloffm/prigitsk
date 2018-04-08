namespace Prigitsk.Core.Simplification
{
    public interface ISimplificationOptions
    {
        bool AggressivelyRemoveFirstBranchNodes { get; }

        bool LeaveTails { get; }

        bool RemoveOrphans { get; }

        bool RemoveOrphansEvenWithTags { get; }
    }
}