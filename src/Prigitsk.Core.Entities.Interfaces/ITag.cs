namespace Prigitsk.Core.Entities
{
    public interface ITag : IPointer
    {
        bool Equals(ITag other);
    }
}