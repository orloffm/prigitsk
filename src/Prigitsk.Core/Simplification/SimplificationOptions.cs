namespace Prigitsk.Core.Simplification
{
    public sealed class SimplificationOptions
        : ISimplificationOptions
    {
        public SimplificationOptions()
        {
            RemoveOrphans = true;
            RemoveOrphansEvenWithTags = true;
            LeaveTails = false;
            AggressivelyRemoveFirstBranchNodes = true;
        }

        public bool AggressivelyRemoveFirstBranchNodes { get; set; }

        public static SimplificationOptions Default => new SimplificationOptions();

        public bool LeaveTails { get; set; }

        public bool RemoveOrphans { get; set; }

        public bool RemoveOrphansEvenWithTags { get; set; }
    }
}