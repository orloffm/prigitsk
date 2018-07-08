using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Graph
{
    public sealed class TagPicker : ITagPicker
    {
        private readonly ITagPickingOptions _options;
        private readonly HashSet<ITag> _tagsToPick;

        public TagPicker(ITagPickingOptions options)
        {
            _options = options;
            _tagsToPick = new HashSet<ITag>();
        }

        public bool CheckIfTagShouldBePicked(ITag tag)
        {
            return _tagsToPick.Contains(tag);
        }

        public void PreProcessAllTags(IEnumerable<ITagInfo> tagInfos)
        {
            _tagsToPick.Clear();

            switch (_options.Mode)
            {
                case TagPickingMode.None:
                    // Don't do anything.
                    return;

                case TagPickingMode.All:
                case TagPickingMode.AllOnMaster:
                case TagPickingMode.Latest:
                    break;

                default:
                    throw new NotSupportedException($"Tag picking mode {_options.Mode.ToString()} is not supported.");
            }

            // First, pick all tags that we will sort by time, if applicable.
            ITagInfo[] allSuitable = SelectAllSuitable(tagInfos).ToArray();

            // Now, if needed, sort by date and pick N last ones.
            if (_options.Mode == TagPickingMode.Latest)
            {
                ITagInfo[] allSuitableOrdered = allSuitable
                    .OrderByDescending(ti => ti.Node.Commit.CommittedWhen)
                    .Take(_options.LatestCount)
                    .ToArray();

                allSuitable = allSuitableOrdered;
            }

            foreach (ITagInfo ti in allSuitable)
            {
                _tagsToPick.Add(ti.Tag);
            }
        }

        private IEnumerable<ITagInfo> SelectAllSuitable(IEnumerable<ITagInfo> tagInfos)
        {
            foreach (ITagInfo tagInfo in tagInfos)
            {
                // If restricted to master, is it on it?
                if (_options.Mode == TagPickingMode.AllOnMaster)
                {
                    bool isOnMaster = string.Equals(
                        tagInfo.ContainingBranch?.Label,
                        "master",
                        StringComparison.OrdinalIgnoreCase);
                    if (!isOnMaster)
                    {
                        continue;
                    }
                }

                // If restricted to non-orphaned branches, do we pass?
                if (!_options.IncludeOrphanedTags)
                {
                    bool isOnOrphanedNode = tagInfo.ContainingBranch == null;
                    if (isOnOrphanedNode)
                    {
                        continue;
                    }
                }

                yield return tagInfo;
            }
        }
    }
}