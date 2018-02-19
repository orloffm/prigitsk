using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.RepoData
{
    public sealed class TagsData : EntityData<ITag>, ITagsData
    {
        public TagsData(IEnumerable<ITag> data) : base(data)
        {

        }
    }
}