using System;
using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Strategy
{
    public sealed class BranchingStrategy : IBranchingStrategy
    {
        private readonly Func<BranchingStrategy, ILesserBranchRegex, IBranchesKnowledge> _knowledgeMaker;

        private readonly ILesserBranchRegex _workItemRegex;

        public BranchingStrategy(
            ILesserBranchRegex workItemRegex,
            Func<BranchingStrategy, ILesserBranchRegex, IBranchesKnowledge> knowledgeMaker)
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