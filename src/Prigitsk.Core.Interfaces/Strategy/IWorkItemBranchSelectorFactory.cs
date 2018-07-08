namespace Prigitsk.Core.Strategy
{
    public interface IWorkItemBranchSelectorFactory
    {
        IWorkItemBranchSelector MakeSelector();
    }
}