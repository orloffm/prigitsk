using System;

namespace Prigitsk.Core.Graph
{
    public sealed class BranchPickerFactory : IBranchPickerFactory
    {
        private readonly Func<IBranchPickingOptions, IBranchPicker> _maker;

        public BranchPickerFactory(Func<IBranchPickingOptions, IBranchPicker> maker)
        {
            _maker = maker;
        }

        public IBranchPicker CreateBranchPicker(IBranchPickingOptions options)
        {
            return _maker(options);
        }
    }
}