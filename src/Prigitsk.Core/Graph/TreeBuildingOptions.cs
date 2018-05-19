namespace Prigitsk.Core.Graph
{
    public class TreeBuildingOptions : ITreeBuildingOptions
    {
        public TreeBuildingOptions()
        {
            TagPickingOptions = new TagPickingOptions()
            {
                Mode = TagPickingMode.All
            };
        }

        public static ITreeBuildingOptions Default => new TreeBuildingOptions();

        public string RemoteToUse => null;

        public bool CheckIfBranchShouldBePicked(string branchLabel)
        {
            return true;
        }

        public ITagPickingOptions TagPickingOptions { get; }
    }
}