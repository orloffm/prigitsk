using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Graph
{
    public sealed class TagPicker : ITagPicker
    {
        private readonly ITagPickingOptions _options;
        private readonly HashSet<ITag> _precachedTags;

        public TagPicker(ITagPickingOptions options)
        {
            _options = options;
            _precachedTags = new HashSet<ITag>();
        }

        public bool CheckIfTagShouldBePicked(ITag tag, IBranch containingBranch)
        {
            switch (_options.Mode)
            {
                case TagPickingMode.None:
                    return false;
                case TagPickingMode.All:
                    return true;
                case TagPickingMode.AllOnMaster:
                    return string.Equals(containingBranch?.Label, "master", StringComparison.OrdinalIgnoreCase);
                case TagPickingMode.Latest:
                    return _precachedTags.Contains(tag);
                default:
                    throw new NotSupportedException($"Tag picking mode {_options.Mode.ToString()} is not supported.");
            }
        }

        public void PreProcessAllTags(IEnumerable<Tuple<ITag, ICommit>> tagsAndNodes)
        {
            _precachedTags.Clear();

            ITag[] passingTags = tagsAndNodes
                .Select(t => new {Tag = t.Item1, Committed = t.Item2.CommittedWhen})
                .Where(p => p.Committed.HasValue)
                .OrderByDescending(p => p.Committed.Value)
                .Select(p => p.Tag)
                .Take(_options.LatestCount)
                .ToArray();

            foreach (ITag tag in passingTags)
            {
                _precachedTags.Add(tag);
            }
        }
    }
}