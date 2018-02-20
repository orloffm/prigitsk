namespace Prigitsk.Core.Simplification
{
    public sealed class SimplificationOptions
        : ISimplificationOptions
    {
        /// <inheritdoc />
        public bool AggressivelyRemoveFirstBranchNodes { get; set; }

        public static SimplificationOptions None => new SimplificationOptions();

        /// <inheritdoc />
        public bool LeaveNodesAfterLastMerge { get; set; }

        /// <inheritdoc />
        public bool PreventSimplification { get; set; }
    }
}