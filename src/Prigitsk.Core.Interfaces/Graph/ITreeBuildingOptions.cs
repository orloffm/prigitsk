namespace Prigitsk.Core.Graph
{
    /// <summary>
    ///     Options for building the initial tree from the repository.
    ///     Does not contain anything related to subsequent simplification process.
    /// </summary>
    public interface ITreeBuildingOptions
    {
        /// <summary>
        ///     Branches from which remote should be used to build the graph.
        /// </summary>
        string RemoteToUse { get; }

        ITagPickingOptions TagPickingOptions { get; }

        bool CheckIfBranchShouldBePicked(string branchLabel);
    }
}