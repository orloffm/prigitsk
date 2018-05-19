using System.Collections.Generic;
using Prigitsk.Core.Entities;
using Prigitsk.Core.RepoData;

namespace Prigitsk.Core.Graph
{
    public interface ITagPickerFactory
    {
        ITagPicker CreateTagPicker(ITagPickingOptions options);
    }
}