namespace Prigitsk.Core.Entities
{
    public interface IHash
    {
        string Value { get; }

        bool Equals(IHash other);
    }
}