namespace Prigitsk.Core.Strategy
{
    /// <summary>
    /// Provides the branching strategy.
    /// </summary>
    public interface IBranchingStrategyProvider
    {
        IBranchingStrategy GetStrategy();
    }
}