using System;

namespace Prigitsk.Core.Strategy
{
    public sealed class WorkItemBranchSelectorFactory : IWorkItemBranchSelectorFactory
    {
        private readonly Func<IWorkItemBranchSelector> _maker;

        public WorkItemBranchSelectorFactory(Func<IWorkItemBranchSelector> maker)
        {
            _maker = maker;
        }

        public IWorkItemBranchSelector MakeSelector()
        {
            return _maker();
        }
    }
}