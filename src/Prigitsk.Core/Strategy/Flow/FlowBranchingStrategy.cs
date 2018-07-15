using System;
using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Strategy.Flow
{
    public sealed class FlowBranchingStrategy : IBranchingStrategy
    {
        private readonly Func<FlowBranchingStrategy, ILesserBranchRegex, IBranchesKnowledge> _knowledgeMaker;

        private readonly ILesserBranchRegex _workItemRegex;

        public FlowBranchingStrategy(
            ILesserBranchRegex workItemRegex,
            Func<FlowBranchingStrategy, ILesserBranchRegex, IBranchesKnowledge> knowledgeMaker)
        {
            _workItemRegex = workItemRegex;
            _knowledgeMaker = knowledgeMaker;
        }

        public IBranchesKnowledge CreateKnowledge(IEnumerable<IBranch> branches)
        {
            IBranchesKnowledge branchesKnowledge = _knowledgeMaker(this, _workItemRegex);
            branchesKnowledge.Initialise(branches);
            return branchesKnowledge;
        }
    }
}