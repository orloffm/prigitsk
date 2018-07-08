namespace Prigitsk.Core.Graph
{
    /// <summary>
    ///     Specifies which tags to pick for the tree.
    /// </summary>
    public interface ITagPickingOptions
    {
        /// <summary>
        ///     Include tags on nodes that are not accessible from any branch pointers.
        /// </summary>
        bool IncludeOrphanedTags { get; }

        /// <summary>
        ///     If picking latest tags, this specified how many to take.
        /// </summary>
        int LatestCount { get; }

        /// <summary>
        ///     The approach to take when picking tags.
        /// </summary>
        TagPickingMode Mode { get; }
    }
}