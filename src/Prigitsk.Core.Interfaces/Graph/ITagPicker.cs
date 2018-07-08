using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Graph
{
    public interface ITagPicker
    {
        /// <summary>
        ///     Returns whether the tag should be included.
        /// </summary>
        bool CheckIfTagShouldBePicked(ITag tag);

        /// <summary>
        ///     Initially looks at the set of all tags to then be able to return information
        ///     for individual ones.
        /// </summary>
        void PreProcessAllTags(IEnumerable<ITagInfo> tagInfos);
    }
}