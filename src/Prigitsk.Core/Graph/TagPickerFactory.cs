using System;

namespace Prigitsk.Core.Graph
{
    public sealed class TagPickerFactory : ITagPickerFactory
    {
        private readonly Func<ITagPickingOptions, ITagPicker> _maker;

        public TagPickerFactory(Func<ITagPickingOptions, ITagPicker> maker)
        {
            _maker = maker;
        }

        public ITagPicker CreateTagPicker(ITagPickingOptions options)
        {
            return _maker(options);
        }
    }
}