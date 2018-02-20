namespace Prigitsk.Core.Tree
{
    public class TreeBuildingOptions : ITreeBuildingOptions
    {
        public static ITreeBuildingOptions Default => new TreeBuildingOptions();

        public string RemoteToUse => null;
    }
}