using System;

namespace Prigitsk.Core.Rendering
{
    public sealed class LesserBranchSelectorFactory : ILesserBranchSelectorFactory
    {
        private readonly Func<ILesserBranchSelector> _maker;

        public LesserBranchSelectorFactory(Func<ILesserBranchSelector> maker)
        {
            _maker = maker;
        }

        public ILesserBranchSelector MakeSelector()
        {
            return _maker();
        }
    }
}