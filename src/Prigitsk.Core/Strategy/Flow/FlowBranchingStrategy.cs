using System;
using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Strategy.Flow
{
    public sealed class FlowBranchingStrategy : IBranchingStrategy
    {
        private readonly Func<FlowBranchingStrategy, IWorkItemSuffixRegex, IEnumerable<IBranch>, IBranchesKnowledge>
            _knowledgeMaker;

        private readonly IWorkItemSuffixRegex _workItemRegex;

        public FlowBranchingStrategy(
            IWorkItemSuffixRegex workItemRegex,
            Func<FlowBranchingStrategy, IWorkItemSuffixRegex, IEnumerable<IBranch>, IBranchesKnowledge> knowledgeMaker)
        {
            _workItemRegex = workItemRegex;
            _knowledgeMaker = knowledgeMaker;
        }

        public IBranchesKnowledge CreateKnowledge(IEnumerable<IBranch> branches)
        {
            return _knowledgeMaker(this, _workItemRegex, branches);
        }
    }
}