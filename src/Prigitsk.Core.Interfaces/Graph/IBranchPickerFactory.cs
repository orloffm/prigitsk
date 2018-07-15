namespace Prigitsk.Core.Graph
{
    public interface IBranchPickerFactory
    {
        IBranchPicker CreateBranchPicker(IBranchPickingOptions options);
    }
}