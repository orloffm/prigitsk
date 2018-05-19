using System;
using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Graph
{
    public interface ITagPicker
    {
        /// <summary>
        ///     Returns whether the tag should be included.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="containingBranch">Branch (if exists) it points to.</param>
        bool CheckIfTagShouldBePicked(ITag tag, IBranch containingBranch);

        /// <summary>
        ///     Initially looks at the set of all tags to then be able to return information
        ///     for individual ones.
        /// </summary>
        /// <param name="tagsAndNodes">All tags and commits they are pointing to.</param>
        void PreProcessAllTags(IEnumerable<Tuple<ITag, ICommit>> tagsAndNodes);
    }
}