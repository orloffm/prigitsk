using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Strategy.Flow
{
    public sealed class FlowBranchesKnowledge : IBranchesKnowledge
    {
        private readonly IBranch[] _branches;
        private readonly IWorkItemBranchSelector _selector;
        private readonly FlowBranchingStrategy _strategy;

        public FlowBranchesKnowledge(
            FlowBranchingStrategy strategy,
            IWorkItemSuffixRegex workItemRegex,
            IEnumerable<IBranch> branches,
            ILesserBranchSelectorFactory lesserBranchSelectorFactory)
        {
            _strategy = strategy;
            _branches = branches.ToArray();
            _selector = lesserBranchSelectorFactory.MakeSelector();

            Initialise(workItemRegex);
        }

        public IBranchingStrategy Strategy => _strategy;

        public IEnumerable<IBranch> EnumerateBranchesInLogicalOrder()
        {
            throw new NotImplementedException();
        }

        public Color? GetSuggestedDrawingColorFor(IBranch branch)
        {
            throw new NotImplementedException();
        }

        public bool IsAWorkItemBranch(IBranch branch)
        {
            throw new NotImplementedException();
        }

        private void Initialise(IWorkItemSuffixRegex workItemRegex)
        {
            _selector.PreProcessAllBranches(_branches, workItemRegex);
        }
    }
}