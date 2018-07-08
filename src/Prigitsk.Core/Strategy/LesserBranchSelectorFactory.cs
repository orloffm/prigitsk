using System;

namespace Prigitsk.Core.Strategy
{
    public sealed class LesserBranchSelectorFactory : ILesserBranchSelectorFactory
    {
        private readonly Func<IWorkItemBranchSelector> _maker;

        public LesserBranchSelectorFactory(Func<IWorkItemBranchSelector> maker)
        {
            _maker = maker;
        }

        public IWorkItemBranchSelector MakeSelector()
        {
            return _maker();
        }
    }
}