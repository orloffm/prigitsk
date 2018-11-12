namespace Prigitsk.Core.Graph
{
    /// <summary>
    ///     Options for building the initial tree from the repository.
    ///     Does not contain anything related to subsequent simplification process.
    /// </summary>
    public interface IBranchPickingOptions
    {
        /// <summary>
        ///     If specified, the branches whose labels match any of these regular expressions are not taken.
        /// </summary>
        string[] ExcludeBranchesRegices { get; }

        /// <summary>
        ///     If specified, only branches whose labels match any of these regular expressions are taken.
        /// </summary>
        string[] IncludeBranchesRegices { get; }
    }
}