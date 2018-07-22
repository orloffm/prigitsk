namespace Prigitsk.Core.Graph
{
    public sealed class TagPickingOptions : ITagPickingOptions
    {
        private TagPickingOptions(TagPickingMode mode, int tagCount, bool includeOrphanedTags)
        {
            LatestCount = tagCount;
            Mode = mode;
            IncludeOrphanedTags = includeOrphanedTags;
        }

        public static TagPickingOptions Default => new TagPickingOptions(TagPickingMode.All, -1, false);

        public bool IncludeOrphanedTags { get; }

        public int LatestCount { get; }

        public TagPickingMode Mode { get; }

        public static ITagPickingOptions Set(TagPickingMode mode, int tagCount, bool includeOrphanedTags)
        {
            return new TagPickingOptions(mode, tagCount, includeOrphanedTags);
        }
    }
}