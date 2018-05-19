namespace Prigitsk.Core.Graph
{
    public sealed class TagPickingOptions : ITagPickingOptions
    {
        public static TagPickingOptions Default => new TagPickingOptions {Mode = TagPickingMode.All};

        public int LatestCount { get; set; }

        public TagPickingMode Mode { get; set; }
    }
}