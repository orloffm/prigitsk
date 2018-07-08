namespace Prigitsk.Core.Simplification
{
    public sealed class SimplificationOptions
        : ISimplificationOptions
    {
        public SimplificationOptions()
        {
            KeepAllOrphans = false;
            LeaveTails = false;
            FirstBranchNodeMayBeRemoved = false;
        }

        public static SimplificationOptions Default => new SimplificationOptions();

        public bool FirstBranchNodeMayBeRemoved { get; set; }

        public bool KeepAllOrphans { get; set; }

        public bool LeaveTails { get; set; }
    }
}