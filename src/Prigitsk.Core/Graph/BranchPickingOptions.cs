namespace Prigitsk.Core.Graph
{
    public sealed class BranchPickingOptions : IBranchPickingOptions
    {
        public static BranchPickingOptions Default => new BranchPickingOptions();

        public bool CheckIfBranchShouldBePicked(string branchLabel)
        {
            return true;
        }
    }
}