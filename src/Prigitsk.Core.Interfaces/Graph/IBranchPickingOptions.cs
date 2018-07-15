namespace Prigitsk.Core.Graph
{
    /// <summary>
    ///     Options for building the initial tree from the repository.
    ///     Does not contain anything related to subsequent simplification process.
    /// </summary>
    public interface IBranchPickingOptions
    {
        string[] IncludeBranchesRegices { get; }
    }
}