namespace Prigitsk.Core.Graph
{
    public sealed class BranchPickingOptions : IBranchPickingOptions
    {
        private BranchPickingOptions(string[] includeBranchesRegices, string[] excludeBranchesRegices)
        {
            IncludeBranchesRegices = includeBranchesRegices;
            ExcludeBranchesRegices = excludeBranchesRegices;
        }

        public string[] ExcludeBranchesRegices { get; }

        public string[] IncludeBranchesRegices { get; }

        public static IBranchPickingOptions Set(string[] includeBranchesRegices, string[] excludeBranchesRegices)
        {
            return new BranchPickingOptions(includeBranchesRegices, excludeBranchesRegices);
        }
    }
}