using System;

namespace Prigitsk.Core.Strategy
{
    public sealed class BranchingStrategyProvider : IBranchingStrategyProvider
    {
        private readonly Func<ILesserBranchRegex, IBranchingStrategy> _strategyMaker;

        public BranchingStrategyProvider(Func<ILesserBranchRegex, IBranchingStrategy> strategyMaker)
        {
            _strategyMaker = strategyMaker;
        }

        public IBranchingStrategy GetStrategy(ILesserBranchRegex workItemRegex = null)
        {
            return _strategyMaker(workItemRegex);
        }
    }
}