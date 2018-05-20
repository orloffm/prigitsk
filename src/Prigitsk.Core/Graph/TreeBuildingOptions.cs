namespace Prigitsk.Core.Graph
{
    public sealed class TreeBuildingOptions : ITreeBuildingOptions
    {
        public TreeBuildingOptions()
        {
            TagPickingOptions = new TagPickingOptions
            {
                Mode = TagPickingMode.All
            };
        }

        public static TreeBuildingOptions Default => new TreeBuildingOptions();

        public string RemoteToUse => null;

        public ITagPickingOptions TagPickingOptions { get; }

        public bool CheckIfBranchShouldBePicked(string branchLabel)
        {
            return true;
        }
    }
}