namespace Prigitsk.Core.Graph
{
    public sealed class BranchPickingOptions : IBranchPickingOptions
    {
        private BranchPickingOptions(string[] includeBranchesRegices)
        {
            IncludeBranchesRegices = includeBranchesRegices;
        }

        public string[] IncludeBranchesRegices { get; }

        public static IBranchPickingOptions Set(string[] includeBranchesRegices)
        {
            return new BranchPickingOptions(includeBranchesRegices);
        }
    }
}