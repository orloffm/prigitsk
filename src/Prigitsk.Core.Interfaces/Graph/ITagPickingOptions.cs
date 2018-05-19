namespace Prigitsk.Core.Graph
{
    /// <summary>
    ///     Specifies which tags to pick for the tree.
    /// </summary>
    public interface ITagPickingOptions
    {
        /// <summary>
        ///     If picking latest tags, this specified how many to take.
        /// </summary>
        int LatestCount { get; set; }

        /// <summary>
        ///     The approach to take when picking tags.
        /// </summary>
        TagPickingMode Mode { get; set; }
    }
}