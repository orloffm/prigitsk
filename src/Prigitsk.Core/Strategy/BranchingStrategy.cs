using System;
using System.Collections.Generic;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Strategy.Flow;

namespace Prigitsk.Core.Strategy
{
    public sealed class BranchingStrategy : IBranchingStrategy
    {
        private readonly Func<BranchingStrategy, ILesserBranchRegex,  IBranchesColorsAndRegices,IEnumerable<IBranch>, IBranchesKnowledge> _knowledgeMaker;

        private readonly ILesserBranchRegex _workItemRegex;
        private readonly IBranchesColorsAndRegices _colorsAndRegices;

        public BranchingStrategy(
            ILesserBranchRegex workItemRegex,
            IBranchesColorsAndRegices colorsAndRegices,
            Func<BranchingStrategy, ILesserBranchRegex, IBranchesColorsAndRegices, IEnumerable<IBranch>, IBranchesKnowledge> knowledgeMaker)
        {
            _workItemRegex = workItemRegex;
            _colorsAndRegices = colorsAndRegices;
            _knowledgeMaker = knowledgeMaker;
        }

        public IBranchesKnowledge CreateKnowledge(IEnumerable<IBranch> branches)
        {
            IBranchesKnowledge branchesKnowledge = _knowledgeMaker(this, _workItemRegex, _colorsAndRegices, branches);
            return branchesKnowledge;
        }
    }
}