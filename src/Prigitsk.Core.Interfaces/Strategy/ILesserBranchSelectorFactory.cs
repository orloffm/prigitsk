namespace Prigitsk.Core.Strategy
{
    public interface ILesserBranchSelectorFactory
    {
        IWorkItemBranchSelector MakeSelector();
    }
}