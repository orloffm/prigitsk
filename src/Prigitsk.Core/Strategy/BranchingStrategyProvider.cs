using System;

namespace Prigitsk.Core.Strategy
{
    public sealed class BranchingStrategyProvider : IBranchingStrategyProvider
    {
        private readonly Func<IWorkItemSuffixRegex, IBranchingStrategy> _strategyMaker;

        public BranchingStrategyProvider(Func<IWorkItemSuffixRegex, IBranchingStrategy> strategyMaker)
        {
            _strategyMaker = strategyMaker;
        }

        public IBranchingStrategy GetStrategy(IWorkItemSuffixRegex workItemRegex = null)
        {
            return _strategyMaker(workItemRegex);
        }
    }
}