namespace Prigitsk.Core.Graph
{
    /// <summary>
    ///     The approach to take when picking tags.
    /// </summary>
    public enum TagPickingMode
    {
        /// <summary>
        ///     Don't take any tags.
        /// </summary>
        None,

        /// <summary>
        ///     Take all tags.
        /// </summary>
        All,

        /// <summary>
        ///     Only tags on master branch.
        /// </summary>
        AllOnMaster,

        /// <summary>
        ///     Take a specified number of newest tags (by commit's timestamps).
        /// </summary>
        Latest
    }
}