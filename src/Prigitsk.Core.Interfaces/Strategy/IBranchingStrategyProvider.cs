namespace Prigitsk.Core.Strategy
{
    /// <summary>
    ///     Provides the branching strategy for the repository.
    /// </summary>
    public interface IBranchingStrategyProvider
    {
        /// <summary>
        ///     Provides the branching strategy for the repository.
        ///     Currently returns the common flow branching strategy.
        ///     This is the point to inject different strategies in the future.
        /// </summary>
        /// <param name="workItemRegex">
        ///     See <see cref="IWorkItemSuffixRegex" /> for information.
        /// </param>
        IBranchingStrategy GetStrategy(IWorkItemSuffixRegex workItemRegex = null);
    }
}