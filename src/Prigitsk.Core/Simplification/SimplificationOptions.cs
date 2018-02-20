namespace Prigitsk.Core.Simplification
{
    public sealed class SimplificationOptions
        : ISimplificationOptions
    {
        /// <inheritdoc />
        public bool AggressivelyRemoveFirstBranchNodes { get; set; }

        /// <inheritdoc />
        public bool LeaveNodesAfterLastMerge { get; set; }

        public static SimplificationOptions None => new SimplificationOptions();

        /// <inheritdoc />
        public bool PreventSimplification { get; set; }
    }
}