namespace Prigitsk.Core.Graph
{
    public interface IBranchPicker
    {
        bool ShouldBePicked(string branchLabel);
    }
}