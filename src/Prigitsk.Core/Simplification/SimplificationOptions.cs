namespace Prigitsk.Core.Simplification
{
    public sealed class SimplificationOptions
        : ISimplificationOptions
    {
        public SimplificationOptions()
        {
            RemoveOrphans = true;
            RemoveOrphansEvenWithTags = true;
            LeaveNodesAfterLastMerge = false;
            AggressivelyRemoveFirstBranchNodes = true;
        }

        public bool AggressivelyRemoveFirstBranchNodes { get; set; }

        public static SimplificationOptions Default => new SimplificationOptions();

        public bool LeaveNodesAfterLastMerge { get; set; }

        public bool RemoveOrphans { get; set; }

        public bool RemoveOrphansEvenWithTags { get; set; }
    }
}