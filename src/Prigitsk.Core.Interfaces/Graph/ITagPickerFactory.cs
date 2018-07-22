namespace Prigitsk.Core.Graph
{
    public interface ITagPickerFactory
    {
        ITagPicker CreateTagPicker(ITagPickingOptions options);
    }
}