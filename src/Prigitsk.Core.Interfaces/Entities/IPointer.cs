namespace Prigitsk.Core.Entities
{
    public interface IPointer
    {
        IHash Tip { get; }
        string Name { get; }
    }
}