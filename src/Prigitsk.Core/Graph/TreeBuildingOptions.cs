namespace Prigitsk.Core.Graph
{
    public class TreeBuildingOptions : ITreeBuildingOptions
    {
        public static ITreeBuildingOptions Default => new TreeBuildingOptions();

        public string RemoteToUse => null;

        public bool CheckIfBranchShouldBePicked(string branchLabel)
        {
            return true;
        }

        public bool CheckIfTagShouldBePicked(string tagName)
        {
            return true;
        }
    }
}