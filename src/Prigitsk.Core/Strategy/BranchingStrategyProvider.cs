namespace Prigitsk.Core.Strategy
{
    public class BranchingStrategyProvider : IBranchingStrategyProvider
    {
        public IBranchingStrategy GetStrategy()
        {
            return new CommonFlowBranchingStrategy();
        }
    }
}